using UnityEngine;
using ZeroX.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
// using Sirenix.OdinInspector;
using ZeroX;

public enum CellType
{
	None,
	Empty,
	Tube,
	Wall,
}

[Serializable] public class CellData
{
	public CellType Type;
	
	public Cell cell;

	public List<Vector2Int> neigbors;
	
	[ShowFieldIf(nameof(Type),  CellType.Tube)]
	public List<int> colors;
	
	[ShowFieldIf(nameof(Type), CellType.Tube)]
	public Tube tube;	
	
	
}

[CreateAssetMenu(fileName = "SOLevelConfig", menuName = "Scriptable Objects/SOLevelConfig")]
public class SOLevelConfig : ScriptableObject
{
	[Header("Level Info")]
	public int        levelID;
	public int        pathID;
	public int        target;
	public int      timer;
	public Grid<CellData> grid;
	public List<int>  colorIDList;
	
	// [Header("Level Difficulty")]
	// [Range(0f, 1f)] public float shuffleIntensity = 0.2f;
	
	
	/*public List<int> GetSuffleColorIDList()
	{
		CalculateColorIDList();
		
		var groups = new List<List<int>>();
		var i      = 0;
		while (i < colorIDList.Count)
		{
			var color = colorIDList[i];
			var count = 1;
			
			while (i + count < colorIDList.Count && colorIDList[i + count] == color)
				count++;
			
			var remaining = count;
			while (remaining > 0)
			{
				var size = UnityEngine.Random.Range(groupSizeRange.x, groupSizeRange.y + 1);
				size = Mathf.Min(size, remaining);
				groups.Add(Enumerable.Repeat(color, size).ToList());
				remaining -= size;
			}
			
			i += count;
		}
		
		var currentShuffleIntensity = shuffleIntensity + ManagerData.CURRENT_LEVEL_MULTIPLIER * 0.1f;
		var shuffleCount            = Mathf.RoundToInt(groups.Count * currentShuffleIntensity);
		for (int j = 0; j < shuffleCount; j++)
		{
			var a = UnityEngine.Random.Range(0, groups.Count);
			var b = UnityEngine.Random.Range(0, groups.Count);
			(groups[a], groups[b]) = (groups[b], groups[a]);
		}
		
		var shuffledList = groups.SelectMany(g => g).ToList();
		return shuffledList;
	} */
	
	public List<int> GetDefaultColorIDList()
	{
		if (colorIDList.Count == 0)
			CalculateColorIDList();
		return colorIDList;
	}
	
	public void CalculateColorIDList()
	{
		colorIDList.Clear();
		// for (int y = 0; y < grid.Height; y++)
		// {
		// 	for (int x = 0; x < grid.Width; x++)
		// 	{
		// 		var tile = grid[x, y];
		// 		if (tile.Type == TileType.Tube)
		// 		{
		// 			for (int k = 0; k < tile.bulletCount; k++)
		// 			{
		// 				colorIDList.Add(tile.colorID);
		// 			}
		// 		}
		// 	}
		// }
	}
	
}