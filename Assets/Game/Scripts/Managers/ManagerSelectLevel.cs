using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ManagerSelectLevel : MonoBehaviour
{
    [SerializeField] private Button buttonHome;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private ButtonLite buttonSettings;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private GameObject popupSettings;

    private void Awake()
    {
        GameEvents.OnCurrentLevelChanged.SubscribeUntilDestroy(OnCurrentLevelChanged, this);
        buttonHome.onClick.AddListener(OnButtonHome);
        buttonPlay.onClick.AddListener(OnButtonPlay);
        buttonSettings.onClick.AddListener(OnButtonSettings);
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

    void OnButtonSettings()
    {
        popupSettings.SetActive(true);
    }
}
