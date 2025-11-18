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
    
    [SerializeField] private Image progressFill;
    [SerializeField] private TextMeshProUGUI txtProgress;

    [SerializeField] private TextMeshProUGUI txtTimer;
    
    [SerializeField] private List<Popup> PopupList;
    
    #endregion
    
    #region Member Variables
    
    #endregion
    
    #region Properties
    
    #endregion
    
    #region Unity Methods
    
    #endregion
    
    #region Public Methods
    
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
    
    public void UpdateProgress(float progress)
    {
        txtProgress.text = $"{(int)(progress*100f)}%";
        progressFill.DOFillAmount(progress, 0.1f).SetEase(Ease.Linear);
    }

    public void UpdateTimer(int seconds)
    {
        var time = Countdowner.TimeFormat3(seconds);
        txtTimer.text = $"{time}";
    }
    #endregion
    
    #region Protected Methods
    
    #endregion
    
    #region Private Methods
    
    #endregion
}
