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

    public Action OnDoneMove;


    private SOGameFeelsSettings _settings;
    private float _playerRollTime;
    private Vector2Int _gridPosition;
    
    private bool _active = false;
    public bool Active => _active;
    public void Initialize()
    {
        _settings = ManagerGame.Instance.GameFeelsSettings;
        _playerRollTime = _settings.PlayerRollTime;
        _active = true;
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
            Vector3 rotateAxis = GetRotateAxis(directionIndex);
            Vector3 nextPosition = GetNextPosition(directionIndex);
            var angle = directionIndex == 2 ? -90 : 90; 
            
            _moveTween = transform.DOMove(nextPosition,_playerRollTime).SetEase(Ease.Linear);
            
            _rotateTween = baseModel.DoRotateAround(rotateAxis,angle, _playerRollTime).SetEase(Ease.Linear);
            
            _upTween = baseModel.DOLocalMoveY(.95f +.25f,_playerRollTime/2).SetEase(Ease.Linear);
            _upTween.OnComplete(() =>
            {
                _downTween = baseModel.DOLocalMoveY(.95F,_playerRollTime/2).SetEase(Ease.Linear);
            });

            yield return Yielder.Wait(_playerRollTime);
            ClearAnimation();
            
            OnDoneMove.Invoke();
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
        _active = false;
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
