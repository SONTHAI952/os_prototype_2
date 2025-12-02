using System.Collections.Generic;
using CS;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZeroX.SingletonSystem;

public enum PopupType
{
    Settings,
    Restart,
    Victory,
    Lose,
}

[System.Serializable]
public class Popup
{
    public PopupType  popupType;
    public GameObject popupPanel;
}


public class ManagerUI : Singleton_ManualSpawn<ManagerUI>
{
    #region Inspector Variables
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] private Button startButton;
    [SerializeField] private List<Popup> PopupList;
    
    #endregion
    
    #region Member Variables
    
    #endregion
    
    #region Properties
    
    #endregion
    
    #region Unity Methods

    protected override void Awake()
    {
        GameEvents.OnStartPlaying.SubscribeOnceUntilDestroy(OnStartPlaying,this);
        startButton.onClick.AddListener(OnButtonStart);
        base.Awake();
    }

    #endregion
    
    public void Init()
    {
        startButton.gameObject.SetActive(true);
    }
    public void OpenPopup(PopupType popupType)
    {
        PopupList?.ForEach(popup => popup.popupPanel.SetActive(popup.popupType == popupType));
        //Debug.Log($"UIPopupController -> OpenPopup -> {popupType}");
    }
    
    public void CloseAllPopup()
    {
        PopupList.ForEach(popup => popup.popupPanel.SetActive(false));
    }
    
    public GameObject GetAndOpenPopup(PopupType popupType)
    {
        if (PopupList == null) return null;
        GameObject selectedPopup = null;
        
        foreach (var popup in PopupList)
        {
            if(popup.popupType == popupType)
            {
                popup.popupPanel.SetActive(true);
                selectedPopup = popup.popupPanel;
            }
            else
            {
                popup.popupPanel.SetActive(false);
            }
        }
        return selectedPopup;
    }

    public void UpdateTimer(int seconds)
    {
        txtTimer.text = seconds == 1 ? "GO" :$"{seconds-1}";
    }

    private void OnButtonStart()
    {
        startButton.gameObject.SetActive(false);
        countDownPanel.gameObject.SetActive(true);
        ManagerGame.Instance.StartCoundown();
    }
    
    private void OnStartPlaying()
    {
        countDownPanel.gameObject.SetActive(false);
    }
    
}
