using System;
using UnityEngine;

public enum CellType
{
    None,
    Land,
    Tree,
    Wood,
    Fish,
    Start,
    Finish
}

[Serializable] 
public class CellData
{
    public CellType Type;
    public Cell cell;
}

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private ParticleSystem woodParticles;
    
    public const int Ball_To_Score = 5;
    public const float CELL_SIZE_X = 1f;
    public const float CELL_SIZE_Z = 1f;
    public bool isMerging;

    int _id;
    CellType _state = CellType.None;
    CellData _data;
    
    public int Id => _id;
    public CellType State => _state;
    
    public void Initialize(int id, CellData data)
    {
        _id = id;
        _data = data;
        _state = _data.Type;
        InitType(_data.Type);
    }

    void InitType(CellType type)
    {
        switch (type)
        {
            case CellType.Land:
                break;
        }
    }

    public bool IsEmpty()
    {
        return _state == CellType.Land;
    }
}
