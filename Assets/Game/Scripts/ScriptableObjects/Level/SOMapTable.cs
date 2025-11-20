using UnityEngine;
using ZeroX.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public enum PatternGroupType
{
	SW,
	A,
	B,
	C,
	D
}

[CreateAssetMenu(fileName = "SOMapTable", menuName = "Scriptable Objects/SOMapTable")]
public class SOMapTable : ScriptableObject
{
	public List<MapPatternGroup> groups;
	public Grid<CellData> GetPatternByGroup(PatternGroupType groupType, int difficulty)
	{
		Grid<CellData> patterns;
		

		return null;
	}
}

[Serializable]
public class MapPatternGroup
{
	public PatternGroupType groupType;
	public List<MapPattern> maps;
	
	public MapPattern GetPatternById(int id)
	{
		return null;
	}
	
	public MapPattern GetRandomPatternByDifficult(int difficulty)
	{
		return null;
	}
}


[Serializable]
public class MapPattern
{
	public Grid<CellData> patterns;
	public int diffucult;
}
