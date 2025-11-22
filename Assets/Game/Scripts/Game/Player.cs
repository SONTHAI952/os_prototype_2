using System;
using System.Collections;
using CS;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using ZeroX.Extensions;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform baseModel;
    [SerializeField] private Collider hitBox;
    [SerializeField] private Rigidbody body;
    


    private SOGameFeelsSettings _settings;
    private float _playerRollTime;
    
    private Vector2Int _gridPosition;
    public void Initialize()
    {
        _settings = ManagerGame.Instance.GameFeelsSettings;
        _playerRollTime = _settings.PlayerRollTime;
    }
    Vector3 GetRotateAxis(float directionIndex)
    {
        if (directionIndex == 0)
            return Vector3.right;

        return Vector3.forward;

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
    
    [ContextMenu("Move")]
    public void Move(float id)
    {
        if(IsRolling())
            return;
        
        float directionIndex = id;
        StartCoroutine(TimeLine());

        IEnumerator TimeLine()
        {
            // transform.position = t1.position; 
            var basePos = baseModel.localPosition;
            Vector3 rotateAxis = GetRotateAxis(directionIndex);
            Vector3 nextPosition = GetNextPosition(directionIndex);
            var angle = directionIndex == 2 ? -90 : 90; 
            
            _moveTween = transform.DOMove(nextPosition,_playerRollTime).SetEase(Ease.Linear);
            
            _rotateTween = baseModel.DoRotateAround(rotateAxis,angle, _playerRollTime).SetEase(Ease.Linear);
            
            _upTween = baseModel.DOLocalMoveY(basePos.y +.25f,_playerRollTime/2).SetEase(Ease.Linear);
            _upTween.OnComplete(() =>
            {
                _downTween = baseModel.DOLocalMoveY(basePos.y,_playerRollTime/2).SetEase(Ease.Linear);
            });

            yield return Yielder.Wait(_playerRollTime);
            ClearAnimation();
        }
    }

    private bool IsRolling()
    {
        if(_upTween != null || _downTween != null || _moveTween != null || _rotateTween != null)
            return true;
        return false;
    }
    private void OnCollisionEnter(Collision other)
    {
        
    }

    public void Fall()
    {
        
    }

    void ClearAnimation()
    {
        _moveTween?.Kill();
        _moveTween = null;
        _rotateTween?.Kill();
        _rotateTween = null;
        _upTween?.Kill();
        _upTween = null;
        _downTween?.Kill();
        _downTween = null;
    }
}
