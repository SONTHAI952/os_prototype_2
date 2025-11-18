using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using ZeroX;
using ZeroX.Variables;

[Serializable]
public class LevelCell
{
    public CellType cellType;
    
    [FormerlySerializedAs("candy")] [FormerlySerializedAs("turret")] [ShowFieldIf(nameof(cellType), CellType.Tube)] 
    public Ball ball;
    
    [HideInInspector]     
    public Transform turretGround;
    
    public void    Clear()       => ball = null;
    public Vector3 GetPosition() => turretGround.position;
    public bool    IsAvailable() => cellType == CellType.Tube && ReferenceEquals(ball, null);
}

public class Level : MonoBehaviour
{
    #region Inspector Variables
    
    public Grid<LevelCell> levelGrid;

    #endregion
    
    #region Member Variables
    
    #endregion
    
    #region Properties
    
    #endregion
    
    #region Unity Methods
    
    #endregion
    
    #region Public Methods
    
    #endregion
    
    #region Protected Methods
    
    #endregion
    
    #region Private Methods
    
    #endregion
}
