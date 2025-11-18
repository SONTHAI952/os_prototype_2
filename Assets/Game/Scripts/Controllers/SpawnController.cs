using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    [SerializeField] List<Cell> spawnCells = new List<Cell>();

    private List<int>[] _tubeDatas;
    private List<int> _colorList;
    private int _tubeCount;
    
    private void Awake()
    {
        GameEvents.OnTubePutDown.SubscribeUntilDestroy(OnTubePutDown, this);
    }

    public void Initialize(List<int> colorList)
    {
        _colorList = colorList;
        SpawnTubes();
    }

    void OnTubePutDown(Tube tube)
    {
        _tubeCount++;
        spawnCells.ForEach(c =>
        {
            if (c.Tube == tube)
                c.RemoveTube();
        });
        
        if(_tubeCount>=3)
            SpawnTubes();
    }
    
    public void SpawnTubes()
    {
        _tubeCount = 0;
        CreateTubeData();
        for(int i = 0; i<_tubeDatas.Length ; i++) 
        {
            spawnCells[i].AnimatedSpawnTube(_tubeDatas[i]);
        }
    }

    void CreateTubeData()
    {
        _tubeDatas = new List<int>[3];

        for (int i = 0; i < _tubeDatas.Length; i++)
        {
            _tubeDatas[i] = new List<int>();

            // Random số phần tử (1 - 2)
            int elementCount = Random.Range(1, 3);

            // Random số lượng loại màu (1 - 2)
            int colorTypeCount = Random.Range(1, 3);

            // Nguồn màu gốc
            List<int> allColors = new List<int>();
            allColors.AddRange(_colorList);
            List<int> chosenColors = new List<int>();

            // Nếu chỉ còn 1 màu duy nhất trong allColors thì chọn luôn nó
            if (allColors.Count == 1)
            {
                int color = allColors[0];
                for (int k = 0; k < elementCount; k++)
                    _tubeDatas[i].Add(color);
                // xong ống này, sang ống tiếp theo
                continue; 
            }

            // Nếu còn nhiều hơn 1 màu → chọn random theo colorTypeCount
            for (int j = 0; j < colorTypeCount; j++)
            {
                int index = Random.Range(0, allColors.Count);
                chosenColors.Add(allColors[index]);
                allColors.RemoveAt(index);
            }

            // Nếu chỉ có 1 loại màu trong ống này → fill toàn bộ ống bằng màu đó
            if (colorTypeCount == 1)
            {
                for (int k = 0; k < elementCount; k++)
                    _tubeDatas[i].Add(chosenColors[0]);
                continue;
            }

            // Nếu có >1 loại màu → chia phần tử cho các block màu
            int remaining = elementCount;
            int[] colorCounts = new int[colorTypeCount];

            for (int j = 0; j < colorTypeCount; j++)
            {
                if (j == colorTypeCount - 1)
                {
                    colorCounts[j] = remaining;
                }
                else
                {
                    colorCounts[j] = Random.Range(1, remaining - (colorTypeCount - j - 1) + 1);
                    remaining -= colorCounts[j];
                }
            }

            // Đảo thứ tự các block màu (nhưng không trộn lẫn phần tử)
            Shuffle(chosenColors);

            // Ghép block vào list
            for (int j = 0; j < colorTypeCount; j++)
            {
                for (int k = 0; k < colorCounts[j]; k++)
                {
                    _tubeDatas[i].Add(chosenColors[j]);
                }
            }
        }
        
        for (int i = 0; i < _tubeDatas.Length; i++)
        {
            Debug.Log($"Tube {i}: " + string.Join(", ", _tubeDatas[i]));
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
