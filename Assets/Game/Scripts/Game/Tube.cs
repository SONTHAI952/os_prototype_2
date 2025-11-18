using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using GDTools.ObjectPooling;
using UnityEngine;
using UnityEngine.Serialization;

public class Tube : MonoBehaviour
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Transform container;
    [SerializeField] private Transform topPivot;
    [SerializeField] private ParticleSystem glassParticles;
    
    public PoolObject PoolObject;
    public bool PickUpable = false;
    public List<Ball> balls;

    private bool _piority = false;
    private Vector3 originPosition;
    private const float Ball_Padding = .5f;
    private float m_JumpTime = .5f;
    private float m_RotateTime = .45f;
    
    public bool HasPiority => _piority;
    public Transform TopPivot => topPivot;
    
    public Vector3 OriginPosition => originPosition;
    
    public void InitializeInBoard(List<int> colorIDs)
    {
        SetOriginPosition();
        SpawnBalls(colorIDs);
    }
    
    public void InitializeInSpawn(List<int> colorIDs)
    {
        PickUpable = true;
        _piority = true;
        SetOriginPosition();
        baseTransform.localScale = Vector3.one;
        transform.eulerAngles = Vector3.zero;
        SpawnBalls(colorIDs);
    }

    public void SetOriginPosition()
    {
        originPosition = transform.position;
    }
    
    void SpawnBalls(List<int> colorIDs)
    {
        for (int i = 0; i < colorIDs.Count; i++)
        {
            var ball = ManagerGame.Instance.PoolController.InstantiateBall(Vector3.zero,colorIDs[i]);
            
            Vector3 ballPosInTube = container.localPosition;
            ballPosInTube.y = Ball_Padding * i;
            ball.transform.SetParent(container);
            ball.transform.localPosition = ballPosInTube;
            balls.Add(ball);
        }
    }
    
    public void RollBackOriginPosition(bool imediate = false)
    {
        if (imediate)
            transform.localPosition = originPosition;
        else
            transform.DOLocalMove(originPosition, .2f);
    }
    public IEnumerator RollBackOriginPositionAndRotation()
    {
        transform.DOLocalMove(originPosition, .2f);
        yield return transform.DORotate(Vector3.zero, .2f).WaitForCompletion();
    }

    public bool IsContainBall()
    {
        return balls.Count > 0;
    }
    
    public int GetTopBallColorID()
    {
        if (balls.Count == 0)
            return -1;
        
        return balls[balls.Count - 1].ColorID;
    } 
    
    public int GetSecondBallColorID()
    {
        if (balls.Count == 0)
            return -1;

        for (int i = balls.Count - 1; i >= 0; i--)
        {
            if (balls[i].ColorID != balls[balls.Count - 1].ColorID)
                return balls[i].ColorID;
        }

        return -1;
    }
    
    public void TransferBall(Ball ball, Vector3 originTopPivotPosition)
    {
        balls.Add(ball);
        ball.SetOwner(this);
        ball.transform.SetParent(transform);
        ball.transform.localScale = Vector3.one;

        //Path Move
        
        Vector3 localTargetPos = new Vector3(0, balls.Count * Ball_Padding, 0);

        Vector3[] path = new Vector3[3];
        path[0] = container.InverseTransformPoint(originTopPivotPosition);  
        path[1] = topPivot.localPosition;
        path[2] = localTargetPos;
        // ball.transform.DOLocalJump(localTargetPos, 5, 1, m_JumpTime).SetEase(Ease.OutSine).OnComplete(() =>
        // {
        //     UpdateTubeLength();
        // });
        ball.transform.DOLocalPath(path, m_JumpTime).SetEase(Ease.InCubic).OnComplete(() =>
        {
            ManagerSounds.Instance.PlaySound(SoundType.Explode);
            UpdateTubeLength();
        });
       
        //Local Rotate
        Vector3 point1 = ball.transform.position;
        point1.y = 0;
        Vector3 point2 = transform.position;
        point2.y = 0;
            
        Vector3 dir = point2 - point1;
        Vector3 rotateAxis = Quaternion.Euler(0, 90, 0) * dir;
        rotateAxis = ball.transform.parent.InverseTransformDirection(rotateAxis); //Vì xoay trong localSpace nên axis cũng cần convert về localSpace
            
        DOVirtual.Float(0, 180, m_RotateTime, v =>
        {
            ball.transform.localRotation = Quaternion.AngleAxis(v, rotateAxis);
        }).OnComplete(()=>
        {
            ball.transform.localEulerAngles = Vector3.zero;
        });
    }

    public IEnumerator MoveToTarget(Tube target)
    {
        //Lấy hướng
        float posX = transform.position.x - target.transform.position.x;
        bool onLeft = posX > 0;
        
        //Setup vị trí
        float offsetX = onLeft ? 2.15f : -2.15f;
        Vector3 targetPos = target.TopPivot.position;
        targetPos.x += offsetX;
        targetPos.y += 2f;
        targetPos.z += 2.15f;
        
        //Setup góc quay
        float rotY = onLeft ? -45f : 45;
        float rotZ = onLeft ? 90f : -90f;
        Vector3 targetAngle = new Vector3(0,rotY, rotZ);
        
        //Chạy anim
        var rotateTween = transform.DORotate(targetAngle, .2f).SetEase(Ease.Linear).WaitForCompletion();
        var moveTween = transform.DOMove(targetPos, .2f).SetEase(Ease.Linear).WaitForCompletion();
        yield return rotateTween;
        yield return moveTween;
    }
    
    public void RemoveBallAt(int index)
    {
        balls.RemoveAt(index);
    }

    public bool IsEmpty()
    {
        if (balls.Count == 0)
        {
            SelfDestroy();
            return true;
        }
        return false;
    }
    
    public void SelfDestroy()
    {
        ManagerGame.Instance.PoolController.DeInstantiateTube(this);
        Instantiate(glassParticles,transform.position,Quaternion.identity);
    }

    public void UpdateTubeLength()
    {
        if (balls.Count < Cell.Ball_To_Score)
            return;
        
        int redundantCount = balls.Count - Cell.Ball_To_Score;
        
        float bonusLength = .35f;
        float totalBonus =  redundantCount * bonusLength;
        
        float currentLength = baseTransform.localScale.y;
        
        baseTransform.DOScaleY(1+totalBonus,.1f).SetEase(Ease.Linear);
    }

    public void OnPioritySpend()
    {
        _piority = false;
    }
}
