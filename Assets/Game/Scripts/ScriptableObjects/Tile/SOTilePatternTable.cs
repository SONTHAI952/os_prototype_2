using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOTilePatternTable", menuName = "Scriptable Objects/SOTilePatternTable")]
public class SOTilePatternTable : ScriptableObject
{
    public List<SOTilePattern> SOTilePatternList;
}
