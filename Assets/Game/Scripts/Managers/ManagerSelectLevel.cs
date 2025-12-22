using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ManagerSelectLevel : MonoBehaviour
{
    [SerializeField] private Button buttonHome;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private TextMeshProUGUI txtLevel;

    private void Awake()
    {
        GameEvents.OnCurrentLevelChanged.SubscribeUntilDestroy(OnCurrentLevelChanged, this);
        buttonHome.onClick.AddListener(OnButtonHome);
        buttonPlay.onClick.AddListener(OnButtonPlay);
        UpdateUI();
    }

    void OnCurrentLevelChanged()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        txtLevel.text = $"Level {ManagerData.CURRENT_LEVEL_ID}";
    }

    void OnButtonPlay()
    {
        ManagerLoading.Instance.LoadingTo(SceneIndexes.Gameplay);
    }
    
    void OnButtonHome()
    {
        ManagerLoading.Instance.LoadingTo(SceneIndexes.Home);
    }
}
