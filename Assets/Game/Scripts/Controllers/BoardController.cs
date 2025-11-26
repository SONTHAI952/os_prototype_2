using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using UnityEngine;
using UnityEngine.Serialization;
using ZeroX.Extensions;
using ZeroX.Variables;

public class BoardController : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Cell tilePrefab;
    [SerializeField] SOLevelConfig levelConfig;
    [SerializeField] Grid<CellData> grid; 
    
    private List<Cell> _finishLines = new List<Cell>();

    public int Width = 0; 
    public int Length = 0; 
    public const float CELL_SPACING_X = 0f;
    public const float CELL_SPACING_Y = 0f;
    
    private void GenerateGrid()
    {
        grid = levelConfig.GetJoinGrid();
        Width = grid.Width;
        Length = grid.Height;
        int fishPivotCount = 0; 
        
        for (int j = 0; j < Length; j++ )
        {
            for (int i = 0; i < Width; i++)
            {
                if(grid[i, j].Type == CellType.None)
                    continue;
                
                var cell = Instantiate(tilePrefab, container);
                Vector3 localPos = Vector3.zero;
                localPos.x = (Cell.CELL_SIZE_X + CELL_SPACING_X)  * i;
                localPos.z = (Cell.CELL_SIZE_Z + CELL_SPACING_Y) * j;
                cell.transform.localPosition = localPos;
                
                cell.Initialize(grid[i, j]);
                grid[i, j].Cell = cell;
                
                if(cell.Type == CellType.Finish)
                    cell.FinishLine.Init(i, j);

                if (cell.Type == CellType.Fish)
                {
                    fishPivotCount++;
                    ManagerGame.Instance.AddFishPivot(fishPivotCount,cell.transform);
                    if(fishPivotCount >= 2)
                        fishPivotCount = 0;
                }
            }
        }
    }
    
    public void Initialize(SOLevelConfig levelConfig)
    {
        ClearData();
        
        this.levelConfig = levelConfig;
        
        GenerateGrid();
    }
    
    void ClearData()
    {
        ClearGrid();
    } 
    
    private void ClearGrid()
    {
        
    }

    #region Querry

    public CellData GetCell(Vector2Int position, int directionIndex)
    {
        if (position.x < 0 || position.x >= grid.Width || position.y < 0 || position.y >= grid.Height) 
            return null;
        return grid[position.x  , position.y];
    }

    public CellData GetSpawnCell()
    {
        return grid[1, 1];
    }
    #endregion
}

