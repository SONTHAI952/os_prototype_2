using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
    [SerializeField] protected float clickScale = 0.95f;
    [SerializeField] ButtonType buttonType = ButtonType.Normal;

    public bool interactable = true;
    public UnityEvent onClick;
    private UnityEvent onDown = new UnityEvent();
    private UnityEvent onUp = new UnityEvent();

    protected float ZoomOutTime = 0.1f;
    protected Vector3 originScale = Vector3.one;

    bool pointerDown = false;

    protected void Awake()
    {
        originScale = transform.localScale;
    }

    public virtual void SetState(bool enable)
    {
        interactable = enable;

    }

    public virtual void SetColor(Color color)
    {
        var icon = GetComponent<Image>();
        if (icon != null)
            icon.color = color;
    }

    public void AddListener(UnityAction action, bool resetAll = false)
    {
        if (!resetAll)
            onClick.RemoveListener(action);
        else
            onClick.RemoveAllListeners();
        AddEvent(action);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;
        pointerDown = true;
        if (interactable) {
            StartCoroutine("StartClick");
            onDown?.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;
        if (interactable) {
            InvokeOnClick();
            PlaySound();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;
        if (!pointerDown)
            return;

        pointerDown = false;
        StopCoroutine("StartClick");
        onUp?.Invoke();
        transform.localScale = originScale;
    }

    protected virtual void InvokeOnClick()
    {
        if (onClick != null)
            onClick.Invoke();
        // if (HUD.Initialized)
        // {
        //     HUD.Instance.Ignore(true);
        //     DOVirtual.DelayedCall(.2f, () =>
        //     {
        //         HUD.Instance.Ignore(false);
        //     }).SetUpdate(true);
        // }
    }

    protected virtual IEnumerator StartClick()
    {
        if (clickScale == 1)
            yield break;
        float tCounter = 0;

        while (tCounter < ZoomOutTime) {
            tCounter += Time.fixedDeltaTime;
            transform.localScale = Vector3.Lerp(originScale, originScale * clickScale, tCounter / ZoomOutTime);
            yield return null;
        }
    }

    public void PlaySound()
    {
        switch (buttonType)
        {
            case ButtonType.Normal:
                break;
            case ButtonType.Purchase:
                //SoundManager.Instance.PlayPurchaseSound();
                break;
        }
    }
    protected void PlayNormalSound()
    {
    }

    public void AddEvent(UnityAction action)
    {
        this.onClick.AddListener(action);
    }

    public void RemoveEvent(UnityAction action)
    {
        this.onClick.RemoveListener(action);
    }

    public void OnDown(UnityAction action)
    {
        this.onDown.AddListener(action);
    }
    public void OnUp(UnityAction action)
    {
        this.onUp.AddListener(action);
    }

}

public enum ButtonType {
    Normal,
    Tab,
    Other,
    Purchase,
}