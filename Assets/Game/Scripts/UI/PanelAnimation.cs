using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PanelAnimation : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private Transform    popupPanel;
    [SerializeField] private List<Button> closeButtons = new();
    [SerializeField] private bool         isAnimate = true;

    #endregion

    #region Member Variables

    private   CanvasGroup canvasGroup;
    protected Action      onPanelOpenAction;
    protected Action      onPanelCloseAction;

    #endregion

    #region Properties
    #endregion

    #region Unity Methods

    protected void Awake()
    {
        canvasGroup = popupPanel.GetComponent<CanvasGroup>();

        if (closeButtons is { Count: > 0 }) 
        {
            foreach (var closeButton in closeButtons)
            { closeButton.onClick.AddListener(AutoClosePanel); }  
        }
    }

    protected void OnEnable()
    {
        if (canvasGroup != null)
        { canvasGroup.interactable = false; }


        if (isAnimate)
        {
            popupPanel.localScale = Vector3.one * 0.1f;
            popupPanel
                .DOScale(1, 0.25f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    onPanelOpenAction?.Invoke();
                    if (canvasGroup != null)
                        canvasGroup.interactable = true; 
                });
        }
        else
        {
            onPanelOpenAction?.Invoke();
            popupPanel.localScale = Vector3.one;
            if (canvasGroup != null)
                canvasGroup.interactable = true; 
        }
        
    }

    #endregion

    #region Public Methods

    public void AutoClosePanel()
    {
        if (canvasGroup != null)
        { canvasGroup.interactable = false; }

        if(isAnimate)
        {
            popupPanel
                .DOScale(0.1f, 0.25f)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() => 
                {
                    onPanelCloseAction?.Invoke();
                    gameObject.SetActive(false); 
                });
        }
        else
        {
            onPanelCloseAction?.Invoke();
            gameObject.SetActive(false); 
        }
    }

    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    #endregion
}
