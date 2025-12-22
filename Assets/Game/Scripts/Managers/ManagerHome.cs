using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ManagerHome : MonoBehaviour
{
    [SerializeField] Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButton);
    }

    void OnPlayButton()
    {
        ManagerLoading.Instance.LoadingTo(SceneIndexes.Gameplay);
    }
}
