
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSettings : PanelAnimation
{
	[SerializeField] private ToggleSlider toggleSlider;
	[SerializeField] private Button buttonOn;
	[SerializeField] private Button buttonOff;
	[SerializeField] private GameObject iconOn;
	[SerializeField] private GameObject iconOff;
	
	
	protected new void Awake()
	{
		base.Awake();
		GameEvents.OnSettingsChanged.SubscribeUntilDestroy(UpdateUI ,this);
		onPanelOpenAction = () => ManagerGame.Instance?.ActiveGameStatus(false);
		onPanelCloseAction = () => ManagerGame.Instance?.ActiveGameStatus(true);
		buttonOn.onClick.AddListener(OnButtonOn);
		buttonOff.onClick.AddListener(OnButtonOff);
		UpdateUI(ManagerData.SETTINGS_SOUND_ON);
		// if (toggleSlider)
		// {
		// 	toggleSlider.SetToggle(ManagerData.SETTINGS_SOUND_ON);
		// 	toggleSlider.OnValueChanged = OnToggleSliderValueChanged;
		// }
	}
	
	// private void OnToggleSliderValueChanged(bool isOn)
	// {
	// 	ManagerData.SETTINGS_SOUND_ON = isOn;
	// 	GameEvents.OnSettingsChanged.Emit(isOn);
	// }

	private void UpdateUI(bool isOn)
	{
		iconOn.gameObject.SetActive(isOn);
		iconOff.gameObject.SetActive(!isOn);
	}

	private void OnButtonOn()
	{
		ManagerData.SETTINGS_MUSIC_ON = true;
		GameEvents.OnSettingsChanged.Emit(true);

	}

	private void OnButtonOff()
	{
		ManagerData.SETTINGS_MUSIC_ON = false;
		GameEvents.OnSettingsChanged.Emit(false);
	}
}