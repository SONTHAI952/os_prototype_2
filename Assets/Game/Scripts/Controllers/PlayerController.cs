using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] List<Cell> spawnCells = new List<Cell>();

    private List<int>[] _tubeDatas;
    private List<int> _colorList;
    private int _tubeCount;
    
    private void Awake()
    {
    }

    public void Initialize()
    {
    }
}
