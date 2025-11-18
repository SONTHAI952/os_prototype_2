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
    [SerializeField] Pipe pipePrefab;
    [SerializeField] SOLevelConfig levelConfig;
    [SerializeField] Grid<CellData> grid;
    
    private List<Pipe> pipes;
    
    public int Width = 0; 
    public int Length = 0; 
    public const float CELL_SPACING_X = .5f;
    public const float CELL_SPACING_Y = .8f;
    
    private void GenerateGrid()
    {
        grid = levelConfig.grid;
        Width = grid.Width;
        Length = grid.Height;
        
        
        //Căn board về giữa
        Vector3 containerOffset = Vector3.zero;
        containerOffset.x = (Cell.CELL_SIZE_X  * .5f) - ((Width * (Cell.CELL_SIZE_X + CELL_SPACING_X) - CELL_SPACING_X) * .5f);
        containerOffset.z = (Cell.CELL_SIZE_Z  * .5f) - ((Length * (Cell.CELL_SIZE_Z + CELL_SPACING_Y) - CELL_SPACING_Y) * .7f);
        container.localPosition = containerOffset;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Length; j++)
            {
                if(grid[i, j].Type == CellType.None)
                    continue;
                
                var cell = Instantiate(tilePrefab, container);
                Vector3 localPos = Vector3.zero;
                localPos.x = (Cell.CELL_SIZE_X + CELL_SPACING_X)  * i;
                localPos.z = (Cell.CELL_SIZE_Z + CELL_SPACING_Y) * j;
                cell.transform.localPosition = localPos;
                if (i % 2 != 0)
                    cell.transform.SetLocalPositionZ(cell.transform.localPosition.z - .9f);
                
                grid[i, j].cell = cell;
            }
        }

        int id = 0;
        foreach (var c in grid.Cells)
        {
            var neighbor = GetNeighbor(c.neigbors);
            if (c.cell)
            {
                c.cell.Initialize(id, c, neighbor);
                id++;
            }
        }
    }

    private void GeneratePipe()
    {
        pipes = new List<Pipe>();
        HashSet<(int, int)> links = new HashSet<(int,int)>();

        foreach (var c in grid.Cells)
        {
            if (c.cell)
            {
                foreach (var neigbor in c.neigbors)
                {
                    Cell cellA = c.cell;
                    Cell cellB = grid[neigbor.x, neigbor.y].cell;
                    
                    if(cellA == null || cellB == null)
                        continue;
                    
                    int idA = cellA.Id;
                    int idB = cellB.Id;
                    
                    var link = idA < idB ? (idA, idB) : (idB, idA);
                    if (links.Add(link))
                    {
                        var pipe = Instantiate(pipePrefab, container);
                        pipe.Initialize(cellA, cellB);
                        pipes.Add(pipe);
                    }
                }
            }
        }
        
        
        
        
    }
    
    public void Initialize(SOLevelConfig levelConfig)
    {
        ClearData();
        
        this.levelConfig = levelConfig;
        
        GenerateGrid();
        GeneratePipe();
    }
    
    public void CheckAvailableBlocksInBoard()
    {
        int cellAvailableCount = 0;
        foreach (var cell in grid.Cells)
        {
            if (cell != null && cell.Type != CellType.None)
            {
                // Debug.LogError(item.isMerging+ "CheckAvailableBlocksInBoard"+item.IsProcessing);
                if(cell.cell.isMerging || cell.cell.IsProcessing) return;
                    
                if (cell.cell.IsEmpty()) cellAvailableCount++;
            }
                
        }
            
        if(cellAvailableCount < 1) GameEvents.OnOutOfSpace.Emit(GameResult.Lose);
    }

    List<Cell> GetNeighbor(List<Vector2Int> neighborPositions)
    {
        List<Cell> neighbors = new List<Cell>();
        foreach (var position in neighborPositions)
        {
            var cellFound = grid[position.x, position.y].cell; 
            if(cellFound)
                neighbors.Add(cellFound);
        }
        return neighbors;
    }
    
    void ClearData()
    {
        ClearGrid();
        ClearPipe();
    } 
    
    private void ClearGrid()
    {
        // if (m_Grid != null && m_Grid.Length != 0)
        // {
        //     foreach (var grid in m_Grid)
        //     {
        //         if (grid != null)
        //             Destroy(grid.gameObject);
        //     }
        // }
        // m_Grid = null;
    }

    public void ClearPipe()
    {
        if (pipes != null && pipes.Count > 0)
        {
            foreach (var pipe in pipes)
            {
                if (pipe != null)
                    Destroy(pipe.gameObject);
            }
        }
        pipes = null;
    }

    #region Querry

    // public Cell GetCell(int x, int y)
    // {
    //     if (x < 0 || x >= m_Grid.GetLength(0) || y < 0 || y >= m_Grid.GetLength(1)) return null;
    //     else return m_Grid[x, y];
    // }
    //
    // public Cell GetCell(Vector2Int position)
    // {
    //     if (position.x < 0 || position.x >= m_Grid.GetLength(0) || position.y < 0 || position.y >= m_Grid.GetLength(1)) return null;
    //     else return m_Grid[position.x, position.y];
    // }

    // public List<Cell> GetNeigbors()
    // {
    //     
    // }

    #endregion
}
