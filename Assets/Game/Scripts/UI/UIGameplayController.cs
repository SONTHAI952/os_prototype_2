using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayController : MonoBehaviour
{
    #region Inspector Variables
    
    [SerializeField] private ButtonLite buttonRestart;
    [SerializeField] private ButtonLite buttonSettings;
    [SerializeField] private TMP_Text levelText;
    
    #endregion
    
    #region Member Variables
    
    #endregion
    
    #region Properties
    
    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        GameEvents.OnLevelLoaded.SubscribeUntilDestroy(OnLevelLoaded, this);
        if (buttonRestart) buttonRestart.onClick.AddListener(OnButtonRestartClick);
        if (buttonSettings) buttonSettings.onClick.AddListener(OnButtonSettingsClick);
    }
    
    #endregion
    
    #region Public Methods
    
    #endregion
    
    #region Protected Methods
    
    #endregion
    
    #region Private Methods
    
    private void OnLevelLoaded()
    {
        if(levelText) levelText.SetText($"Level {ManagerData.CURRENT_LEVEL_ID}");
    }
    
    private void OnButtonRestartClick()
    {
        ManagerUI.Instance.OpenPopup(PopupType.Restart);
    }
    
    private void OnButtonSettingsClick()
    {
        ManagerUI.Instance.OpenPopup(PopupType.Settings);
    }
    
    #endregion
}
