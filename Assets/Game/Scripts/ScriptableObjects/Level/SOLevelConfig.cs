using UnityEngine;
using ZeroX.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelPatternData
{
	public PatternGroupType groupType;
	public int diffucult;
}

[CreateAssetMenu(fileName = "SOLevelConfig", menuName = "Scriptable Objects/SOLevelConfig")]
public class SOLevelConfig : ScriptableObject
{
	[Header("Level Info")]
	public int        levelID;
	public int        pathID;
	public List<LevelPatternData> patternDatas;

	// [Header("Level Difficulty")]
	// [Range(0f, 1f)] public float shuffleIntensity = 0.2f;




}