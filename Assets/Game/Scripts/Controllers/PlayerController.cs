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
    
    
    public void Initialize()
    {
        _smoothSpeed = ManagerGame.Instance.GameFeelsSettings.CameraSmoothSpeed;
        _player = Instantiate(prefab);
        _player.Initialize();
    }

    public void Move(int directionIndex)
    {
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
}
