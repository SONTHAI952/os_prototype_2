using System.Collections;
using System.Collections.Generic;
using CS;
using DG.Tweening;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField] private Transform prefab;
    
    public List<Transform> _pivots = new List<Transform>();
    public Transform _fish;
    public Vector3[] _paths;
    public Vector3 _p1, _p2, _p3, _p4, _p5;
    
    public void Init()
    {
        if (_pivots.Count < 2)
            return;
        _fish = Instantiate(prefab);
        
        _p1 = _pivots[0].position;
        _p5 = _pivots[1].position;
        
        _p2 = (_p5 + _p1) * 0.2f;
        _p2.y = .7f;
        
        _p3 = (_p5 + _p1) * 0.5f;
        _p3.y = 1.3f;
        
        _p4 = (_p5 + _p1) * 0.8f;
        _p4.y = .7f;
        
        _paths = new[]
        {
            _p1,  _p3,  _p5
        };
        
        RunFishLoop();
    }
    
    public void AddPivot(Transform pivot)
    {
        if(_pivots.Count >= 2)
            return;
        _pivots.Add(pivot);
    }

    public void RunFishLoop()
    {
        StartCoroutine(Timeline());

        IEnumerator Timeline()
        {
            yield return Yielder.Wait(2);
            _fish.position = _paths[0];
            _fish.gameObject.SetActive(true);
            _fish.DOPath(_paths, 1f, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() =>
            {
                _fish.gameObject.SetActive(false);
                RunFishLoop();
            });
        }
    }

    void ClearData()
    {
        foreach (var pivot in _pivots)
        {
            Destroy(pivot.gameObject);
        }
        _pivots.Clear();
    }
}
