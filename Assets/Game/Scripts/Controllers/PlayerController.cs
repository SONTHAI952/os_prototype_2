using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player prefab;
    [SerializeField] private Transform cameraPivot;
    
    private Player _player;
    private float _smoothSpeed;
    private bool _pending = false;
    
    
    public void Initialize()
    {
        _smoothSpeed = ManagerGame.Instance.GameFeelsSettings.CameraSmoothSpeed;
        _player = Instantiate(prefab);
        _player.Initialize();
    }

    public void CheckMove()
    {
        Debug.LogError("Can Move ="+CanMove());
        if(!CanMove())
            return;
        
        _player.Move(0);
    }
    public void MoveByInput(int directionIndex)
    {
        if (_player.IsRolling())
            _player.OnDoneMove += () => _player.Move(directionIndex);
        else
            _player.Move(directionIndex);
    }
    
    void LateUpdate()
    {
        if (_player == null) 
            return;

        //Update Camera
        Vector3 desiredPosition = new Vector3(
            _player.transform.position.x,
            cameraPivot.position.y,               
            _player.transform.position.z 
        );

        cameraPivot.position = Vector3.Lerp(
            cameraPivot.position,
            desiredPosition,
            _smoothSpeed * Time.deltaTime
        );
    }

    private bool CanMove()
    {
        if (_player != null && _player.Active && !_player.IsRolling() && !_pending)
            return true;
        return false;
    }

    public void PendingNextMove(bool value)
    {
        _pending = value;
    }
}
