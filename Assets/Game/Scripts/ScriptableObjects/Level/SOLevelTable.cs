using System.Collections.Generic;
// using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SOLevelTable", menuName = "Scriptable Objects/SOLevelTable")]
public class SOLevelTable : ScriptableObject
{
    public List<SOLevelConfig> levelList;
    public int totalLevelCount => levelList.Count;
    
    // [Button]
    public void CalculateMonsterData()
    {
        foreach (var level in levelList)
        {
            level.CalculateColorIDList();
        }
    }

    public SOLevelConfig GetLevelConfig(int level)
    {
        return levelList.Find(x => x.levelID == level);
    }
}
