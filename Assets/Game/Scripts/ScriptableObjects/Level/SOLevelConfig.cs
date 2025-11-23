using UnityEngine;
using ZeroX.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelPatternData
{
	public PatternGroupType groupType;
	public int id;
	public int diffucult;
}

[CreateAssetMenu(fileName = "SOLevelConfig", menuName = "Scriptable Objects/SOLevelConfig")]
public class SOLevelConfig : ScriptableObject
{
	[Header("Level Info")]
	public int        levelID;
	public int        pathID;
	public List<LevelPatternData> patternDatas;

	public Grid<CellData> GetJoinGrid()
	{
		var grid = new Grid<CellData>();
		var table  = ManagerGame.Instance.MapTable;
		for (int i = 0; i < patternDatas.Count; i++)
		{
			var data = patternDatas[i];
			
			var group = table.GetGroup(data.groupType);
			
			var pattern = data.id == 0 ? 
				group.GetRandomPatternByDifficult(data.groupType,data.diffucult).patterns : 
				group.GetPatternById(data.id).patterns;

			grid.AddExistGrid(pattern); 
		}
		return grid;
	}
	// [Header("Level Difficulty")]
	// [Range(0f, 1f)] public float shuffleIntensity = 0.2f;




}