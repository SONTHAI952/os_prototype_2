using System;
using System.Collections;
using CS;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using ZeroX.Extensions;
using ZeroX.RxSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform baseModel;
    [SerializeField] private Collider hitBox;
    [SerializeField] private Rigidbody body;
    [SerializeField] private GameObject particle;

    public Action OnDoneMove;


    private SOGameFeelsSettings _settings;
    private BoardController _boardController;
    private float _playerRollTime;
    private Vector2Int _gridPosition;
    
    private bool _active = false;
    public bool Active => _active;
    public void Initialize(Vector2Int gridPosition)
    {
        _settings = ManagerGame.Instance.GameFeelsSettings;
        _boardController = ManagerGame.Instance.BoardController;
        _playerRollTime = _settings.PlayerRollTime;
        _gridPosition = gridPosition;
        _active = true;
    }
    Vector3 GetRotateAxis(float directionIndex)
    {
        if (directionIndex == 0)
            return Vector3.right;

        return Vector3.forward;

    }

    Vector2Int GetNextGridPosition(int directionIndex)
    {
        var posIndex = _gridPosition;
        switch (directionIndex)
        {
            case 0:
                posIndex.y += 1;
                break;
            case 1:
                posIndex.x -= 1;
                break;
            case 2:
                posIndex.x += 1;
                break;
        }

        return posIndex;
    }
    Vector3 GetNextPosition(Vector2Int position,int directionIndex)
    {
        var cell = _boardController.GetCell(position, directionIndex);
        if (cell == null || cell.Type == CellType.None)
        {
            _active = false;
            OnDoneMove = null;
            OnDoneMove += () => Fall();
            
            return GetNextPosition(directionIndex);
        }
        else
        {
            return cell.Cell.transform.position;
        }
    }
    
    Vector3 GetNextPosition(float directionIndex)
    {
        var pos = transform.position;
        switch (directionIndex)
        {
            case 0:
                pos.z += 1f;
                break;
            case 1:
                pos.x -= 1f;
                break;
            case 2:
                pos.x += 1f;
                break;
        }
        return pos;
    }
    
    Tween _upTween;
    Tween _downTween;
    Tween _moveTween;
    Tween _rotateTween;
    
    
    public void Move(int id)
    {
        if(IsRolling())
            return;
        
        int directionIndex = id;
        StartCoroutine(TimeLine());

        IEnumerator TimeLine()
        {
            Vector3 rotateAxis = GetRotateAxis(directionIndex);
            Vector2Int nextGridPosition = GetNextGridPosition(directionIndex);
            // Debug.LogError("nextGridPosition "+nextGridPosition);
            Vector3 nextPosition = GetNextPosition(nextGridPosition,directionIndex);
            // Debug.LogError("next Position "+nextPosition);
            var angle = directionIndex == 2 ? -90 : 90; 
            // Debug.LogError("Move Distance: "+(nextPosition - transform.position).magnitude + " id "+directionIndex);
            
            _moveTween = transform.DOMove(nextPosition,_playerRollTime).SetEase(Ease.Linear);
            
            _rotateTween = baseModel.DoRotateAround(rotateAxis,angle, _playerRollTime).SetEase(Ease.Linear);
            
            _upTween = baseModel.DOLocalMoveY(.95f +.25f,_playerRollTime/2).SetEase(Ease.Linear);
            _upTween.OnComplete(() =>
            {
                _downTween = baseModel.DOLocalMoveY(.95F,_playerRollTime/2).SetEase(Ease.Linear);
            });

            yield return Yielder.Wait(_playerRollTime);
            _gridPosition = nextGridPosition;
            ClearAnimation();
            
            OnDoneMove?.Invoke();
            if(Active)
                OnDoneMove = null;
            
        }
    }

    public bool IsRolling()
    {
        if(_upTween != null || _downTween != null || _moveTween != null || _rotateTween != null)
            return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
            Damage();
        else if(other.CompareTag("Finish"))
        {
            GameEvents.OnWin.Emit(GameResult.Win);
        }

    }

    public void Damage()
    {
        ClearAnimation();
        baseModel.gameObject.SetActive(false);
        hitBox.gameObject.SetActive(false);
        Instantiate(particle, transform.position, Quaternion.identity);
        _active = false;
        OnDoneMove = null;
        GameEvents.OnLose.Emit(GameResult.Lose);
    }

    public void Fall()
    {
        body.isKinematic = false;
        _active = false;
    }

    void ClearAnimation()
    {
        _moveTween?.Kill();
        _rotateTween?.Kill();
        _upTween?.Kill();
        _downTween?.Kill();
        _moveTween = null;
        _rotateTween = null;
        _upTween = null;
        _downTween = null;
    }
}
