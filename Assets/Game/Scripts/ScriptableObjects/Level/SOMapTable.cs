using UnityEngine;
using ZeroX.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public enum PatternGroupType
{
	S,
	A,
	B,
	C,
	D,
	W
}

[CreateAssetMenu(fileName = "SOMapTable", menuName = "Scriptable Objects/SOMapTable")]
public class SOMapTable : ScriptableObject
{
	public List<MapPatternGroup> groups;
	public MapPatternGroup GetPatternByGroup(PatternGroupType groupType)
	{
		var group =  groups.Find(g => g.groupType == groupType);
		return group;
	}
}

[Serializable]
public class MapPatternGroup
{
	public PatternGroupType groupType;
	public List<MapPattern> maps;
	
	public MapPattern GetPatternById(int id)
	{
		return maps[id-1];
	}
	
	public MapPattern GetRandomPatternByDifficult(int difficulty)
	{
		List<MapPattern> mapByDifficults = maps.FindAll(m => m.diffucult == difficulty);
		return mapByDifficults.RandomElement();
	}
}


[Serializable]
public class MapPattern
{
	public Grid<CellData> patterns;
	public int diffucult;
}
