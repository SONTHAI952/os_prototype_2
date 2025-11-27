using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public List<GameObject> trees;

    public void Init()
    {
        var tree = trees.RandomElement();
        tree.gameObject.SetActive(true);
    }
}
