using System;
using UnityEngine;
using UnityEngine.Serialization;

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
    public Cell Cell;
}

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Land land;
    [SerializeField] private Tree tree;
    [SerializeField] private Wood wood;
    [SerializeField] private FinishLine finishLine;
    
    public const float CELL_SIZE_X = 1f;
    public const float CELL_SIZE_Z = 1f;
    public bool isMerging;
    

    CellType _type = CellType.None;
    CellData _data;
    
    public CellType Type => _type;
    public Land Land => land;
    public Tree Tree => tree;
    public Wood Wood => wood;
    public FinishLine FinishLine => finishLine;
    
    public void Initialize(CellData data)
    {
        land.gameObject.SetActive(false);
        tree.gameObject.SetActive(false);
        wood.gameObject.SetActive(false);
        
        _data = data;
        _type = _data.Type;
        InitType(_data.Type);
    }

    void InitType(CellType type)
    {
        switch (type)
        {
            case CellType.Land:
                land.gameObject.SetActive(true);
                break;
            case CellType.Tree:
                tree.gameObject.SetActive(true);
                break;
            case CellType.Wood:
                wood.gameObject.SetActive(true);
                break;
            case CellType.Finish:
                finishLine.gameObject.SetActive(true);
                break;
        }
    }

    public void SetupFinishLine(bool white)
    {
        
    }
    public bool IsEmpty()
    {
        return _type == CellType.Land;
    }
}
