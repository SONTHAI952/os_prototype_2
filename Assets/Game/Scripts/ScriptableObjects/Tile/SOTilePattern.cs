using System;
using System.Collections.Generic;
// using Sirenix.OdinInspector;
using UnityEngine;
using ZeroX.Variables;

public enum TileState 
{ 
	None,
	Full,
}

[CreateAssetMenu(fileName = "SOTilePattern", menuName = "Scriptable Objects/SOTilePattern")]
public class SOTilePattern : ScriptableObject
{
	public Grid<TileState> tilePattern = new (3,3);
	public GameObject     wallGameObject;
	
	// [Button]
	public string GetPattern0() => GetPatternRotated(0);
	// [Button]
	public string GetPattern90() => GetPatternRotated(90);
	// [Button]
	public string GetPattern180() => GetPatternRotated(180);
	// [Button]
	public string GetPattern270() => GetPatternRotated(270);
	
	/// <summary>
	/// Sinh ra pattern dạng chuỗi theo góc quay (0,90,180,270)
	/// </summary>
	public string GetPatternRotated(int rotation)
	{
		int[,] map = new int[3, 3];
		for (int x = 0; x < 3; x++)
		for (int y = 0; y < 3; y++)
			map[x, y] = (int)tilePattern[x, y];
		
		int Get(int x, int y)
		{
			return map[x, y];
		}
		
		// Duyệt 8 ô quanh tâm theo chiều kim đồng hồ, bắt đầu từ TL
		Vector2Int[] dirs =
		{
				new(-1, -1), // TL
				new(0, -1),  // T
				new(1, -1),  // TR
				new(1, 0),   // R
				new(1, 1),   // BR
				new(0, 1),   // B
				new(-1, 1),  // BL
				new(-1, 0)   // L
		};
		
		string pattern = "";
		
		foreach (var dir in dirs)
		{
			Vector2Int rdir = RotateDir(dir, rotation);
			int        x    = 1 + rdir.x;
			int        y    = 1 + rdir.y;
			pattern += Get(x, y).ToString();
		}
		
		return pattern;
	}
	
	private Vector2Int RotateDir(Vector2Int dir, int angle)
	{
		angle = (angle + 360) % 360;
		return angle switch
		{
				90  => new Vector2Int(-dir.y, dir.x),
				180 => new Vector2Int(-dir.x, -dir.y),
				270 => new Vector2Int(dir.y,  -dir.x),
				_   => dir
		};
	}
	

}
