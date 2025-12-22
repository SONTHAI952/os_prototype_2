using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : ButtonBase
{
    [SerializeField] private int id;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private Image imageIcon;
    [SerializeField] private Image imageLock;

    private void Start()
    {
        GameEvents.OnCurrentLevelChanged.SubscribeUntilDestroy(OnCurrentLevelChanged, this);
        AddListener(OnClick);
        imageLock.sprite = imageIcon.sprite;
        UpdateUI();
    }

    void OnClick()
    {
        ManagerData.CURRENT_LEVEL_ID = id;
        GameEvents.OnCurrentLevelChanged.Emit();
    }

    void OnCurrentLevelChanged()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        var isLocked = id > ManagerData.MAX_LEVEL_UNLOCKED;
        ToggleLock(isLocked);
    }
    
    void ToggleLock(bool value)
    {
        interactable = !value;
        imageLock.gameObject.SetActive(value);
    }
}
