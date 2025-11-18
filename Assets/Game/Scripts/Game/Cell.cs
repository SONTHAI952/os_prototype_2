using System.Collections;
using System.Collections.Generic;
using CS;
using DG.Tweening;
using GDTools.ObjectPooling;
using Unbound.Core.Extension;
using UnityEngine;
using UnityEngine.Serialization;

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private List<Cell> neighbors;
    [SerializeField] private ParticleSystem woodParticles;
    
    public const int Ball_To_Score = 5;
    public const float CELL_SIZE_X = 1f;
    public const float CELL_SIZE_Z = 1f;
    public bool isMerging;

    int _id;
    CellType _state = CellType.Empty;
    Tube _tube;
    Wall _wall;
    CellData _data;
    
    public int Id => _id;
    public CellType State => _state;
    public Tube Tube => _tube;
    public Wall Wall => _wall;
    
    public void Initialize(int id, CellData data, List<Cell> neighbors)
    {
        _id = id;
        _data = data;
        this.neighbors = neighbors;
        _state = _data.Type;
        InitType(_data.Type);
    }

    void InitType(CellType type)
    {
        switch (type)
        {
            case CellType.Tube:
                _tube = ManagerGame.Instance.PoolController.InstantiateTube(transform.position);
                _tube.InitializeInBoard(_data.colors);
                break;
            case CellType.Wall:
                _wall = ManagerGame.Instance.PoolController.InstantiateWall(transform.position);
                _wall.Initialize();
                break;
        }
    }

    public void AddTube(Tube tube)
    {
        _tube = tube;
        _tube.transform.position = transform.position;
        _tube.SetOriginPosition();
        _tube.PickUpable = false;
        _state =  CellType.Tube;
    }

    public void RemoveTube()
    {
        _tube = null;
    }
    
    //Spawn
    public void AnimatedSpawnTube(List<int> colors)
    {
        _tube = ManagerGame.Instance.PoolController.InstantiateTube(transform.position);
        _tube.InitializeInSpawn(colors);
        _tube.transform.AddLocalPositionZ(-15f);
        _tube.RollBackOriginPosition();
    }
    
    //Board 
    public bool IsProcessing;

    public void AutoMerge()
    {
        if (IsContainTube() == false)
            return;
        var listCellSameColor =  neighbors.FindAll(c => 
            c.IsProcessing == false && 
            c.IsMergeable() &&  
            c._tube.GetTopBallColorID() == _tube.GetTopBallColorID());
        
        if (listCellSameColor.Count == 0)
        {
            ManagerGame.Instance.BoardController.CheckAvailableBlocksInBoard();
            return;
        }

        IsProcessing = true;
        StartCoroutine(Timeline());

        IEnumerator Timeline()
        {
            Cell cellEnd = this;
            

            Debug.Log("auto merge cell point: "+ cellEnd.GetPointToMerge());
            List<Cell> cTempList = new List<Cell>();
            foreach (var c in listCellSameColor)
            {
                c.IsProcessing = true;
                cTempList.Add(c); 
                
                Debug.Log("c point: "+ c.GetPointToMerge()+" cellend point: "+ cellEnd.GetPointToMerge());
                if (c.GetPointToMerge() > cellEnd.GetPointToMerge()) 
                    cellEnd = c;
                
                Debug.Log("WHY c point: "+ c.GetPointToMerge()+" cellend point: "+ cellEnd.GetPointToMerge());
            }

            yield return Yielder.Wait(.1f);
            
            if(cellEnd != this)
            {
                cTempList.Remove(cellEnd);
            }

            foreach (var c in cTempList) 
                yield return c.TryTransferBall(this);
            
            if (cellEnd != this)
            {
                yield return TryTransferBall(cellEnd);
                yield return Yielder.Wait(.1f);
                if (listCellSameColor.Count > 0) 
                    foreach (var i in listCellSameColor) 
                        i.IsProcessing = false;
                
                cellEnd.CheckAutoMerge();
                yield break;
            }
            
            CheckAutoMerge();
        }
    }

    public void CheckAutoMerge()
    {
        if (TryCollectBallsIfEnoughCondition() == false)
        {
            // IsProcessing = false;

            if (IsMergeable())
                AutoMerge();
        }
    }
    
    public IEnumerator TryTransferBall(Cell target)
    {
        if (_tube.balls.Count == 0)
            yield break;

        if (target.IsProcessing)
        {
            int topBallColorID = _tube.GetTopBallColorID();
            int targetTopBallColorID = target._tube.GetTopBallColorID();

            int count = 0;

            if (topBallColorID == targetTopBallColorID)
            {
                yield return _tube.MoveToTarget(target.Tube);
                
                for (int i = _tube.balls.Count - 1; i >= 0; i--)
                {
                    if (i >= _tube.balls.Count)
                    {
                        break;
                    }

                    int index = i;
                    if (_tube.balls[index] == null)
                    {
                        _tube.RemoveBallAt(index);
                        continue;
                    }

                    if (_tube.balls[index].ColorID == topBallColorID)
                    {
                        target._tube.TransferBall(_tube.balls[index],_tube.TopPivot.position);
                        _tube.RemoveBallAt(index);

                        count++;
                        yield return Yielder.Wait(.015f);
                    }
                    else
                    {
                        isMerging = false;
                        target.isMerging = false;
                        break;
                    }
                    isMerging = false;
                    target.isMerging = false;
                    yield return Yielder.Wait(.035f);
                }
                yield return Yielder.Wait(.46f);
                
                if (_tube.IsEmpty())
                {
                    UpdateCellState(CellType.Empty);
                    _tube = null;
                }
                else
                {
                    yield return _tube.RollBackOriginPositionAndRotation();
                }
            }
        }

        IsProcessing = false;
        if (IsMergeable())
        {
            AutoMerge();
        }

    }
    
    public bool TryCollectBallsIfEnoughCondition()
    {
        if (!IsContainTube())
        {
            IsProcessing = false;
            return false;
        }
        int number = 0;
        bool countable = true;
        for (int i = _tube.balls.Count - 1; i >= 0; i--)
        {
            if (_tube.balls[i].ColorID == _tube.balls[_tube.balls.Count - 1].ColorID)
            {
                if(countable)
                    number++;
            }
            else
                countable = false;
        }
    
        if (number >= Ball_To_Score)
        {
            IsProcessing = true;
            // _state = CellType.Empty;
    
            ActionPreRemoveStacks();
            RemoveStacksTopType();
    
            GameEvents.OnBallCollect.Emit();
            return true;
        }
    
        IsProcessing = false;
        return false;
    }
    
    public void RemoveStacksTopType()
    {
        StartCoroutine(Timeline());
    
        IEnumerator Timeline()
        {
            var topType = _tube.GetTopBallColorID();
    
    
            for (int i = _tube.balls.Count - 1; i >= 0; i--)
            {
                // tube.balls[i].Highlight();
                yield return Yielder.Wait(.05f);
            }
    
            int stackRemoveCount = 0;
            for (int i = _tube.balls.Count - 1; i >= 0; i--)
            {
                if (i < _tube.balls.Count && _tube.balls[i].ColorID == topType)
                {
                    // SoundManager.Instance.hexPop.Play();
                    _tube.balls[i].AutoDestroy();
                    _tube.RemoveBallAt(i);
                    _tube.UpdateTubeLength();
                    stackRemoveCount++;
                    yield return Yielder.Wait(.05f);
                }
                else
                {
                    // _state = CellType.Empty;
                    AutoMerge();
                    break;
                }
            }
            // if(LevelManager.IsPlaying) OnAddScore.Emit(stackRemoveCount);
            // if(!LevelManager.IsCompleted)OnTopTypeCollect.Emit(data);
    
            if(!_tube.IsContainBall())
                _state = CellType.Empty;
            
            if(stackRemoveCount != 0 && _state == CellType.Empty)
                ActionAfterRemoveBalls(topType);
    
            stackRemoveCount = 0;
    
    
            // PlayEffectTargetLevel();
    
            IsProcessing = false;
            if (IsMergeable())
                AutoMerge();
        }
    }
    
    private void ActionAfterRemoveBalls(int colorID)
    {
        if (_tube.IsEmpty())
        {
            UpdateCellState(CellType.Empty);
            _tube = null;
        }
        
        foreach (var cell in neighbors)
        {
            if (cell != null && cell.gameObject.activeSelf)
            {
                cell.TakeDamageAfterClearStack(colorID);
            }
        }
    }
    
    public void TakeDamageAfterClearStack(int colorID)
    {
        if (_state == CellType.Wall)
        {
            ManagerGame.Instance.PoolController.DeInstantiateWall(_wall);
            Instantiate(woodParticles,transform);
            _wall = null;
            UpdateCellState(CellType.Empty);
        }
    }
    
    private void ActionPreRemoveStacks()
    {
        foreach (var child in neighbors)
        {
            if (child != null && child.gameObject.activeSelf)
            {
                child.TakeDamagePreClearStack();
            }
        }
    }
    
    public void TakeDamagePreClearStack()
    {
        
    }
    
    public int GetPointToMerge()
    {
        int point = 0;
        if (IsContainTube() && IsOnlyOneColor())
        {
            if (IsOnlyOneColor())
            {
                point += 43;
            }
        
            if (_tube.HasPiority)
            {
                point += 1000;
            }
        }

        int topColor = _tube.GetTopBallColorID();
        int secondColor = _tube.GetSecondBallColorID();

        foreach (var cell in neighbors)
        {
            if (cell != null )
            {
                // if (cell.State == CellType.Wall)
                // {
                //     point += 259;
                // }

                if (cell.IsContainTube())
                {
                    if (cell._tube.GetTopBallColorID() == topColor)
                    {
                        point += 7;
                    }
                    
                    if (secondColor >= 0 && cell._tube.GetTopBallColorID() == secondColor)
                    {
                        point -= 1;
                    }
                }
            }
        }

        return point;
    }
    
    public void UpdateCellState(CellType nextState)
    {
        _state =  nextState;
    }
    
    public bool IsEmpty()
    {
        return _state == CellType.Empty && _tube == null && _wall == null;
    }

    public bool IsOnlyOneColor()
    {
        return _tube.balls.FindAll(x => x.ColorID != _tube.GetTopBallColorID()).Count == 0;
    }
    
    public bool IsMergeable()
    {
        return _state == CellType.Tube && IsContainTube();
    }
    
    public bool IsContainTube()
    {
        return _tube != null && _tube.IsContainBall();
    }

    bool _scaleUp = false;
    public void ScaleUp(bool scaleUp)
    {
        if(_scaleUp == scaleUp)
            return;
        
        _scaleUp = scaleUp;
        
        if (_scaleUp)
            baseTransform.localScale = Vector3.one * 1.2f;
        else 
            baseTransform.localScale = Vector3.one;
    }
}
