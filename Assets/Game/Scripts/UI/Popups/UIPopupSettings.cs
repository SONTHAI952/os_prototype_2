
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSettings : PanelAnimation
{
	#region Inspector Variables
	
	[SerializeField] private ToggleSlider toggleSlider;
	
	#endregion
	
	#region Member Variables
	
	#endregion
	
	#region Properties
	
	#endregion
	
	#region Unity Methods
	
	protected new void Awake()
	{
		base.Awake();
		if (toggleSlider)
		{
			toggleSlider.SetToggle(ManagerData.SETTINGS_SOUND_ON);
			toggleSlider.OnValueChanged = OnToggleSliderValueChanged;
		}
		onPanelOpenAction = () => ManagerGame.Instance.ActiveGameStatus(false);
		onPanelCloseAction = () => ManagerGame.Instance.ActiveGameStatus(true);
	}
	
	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Protected Methods
	
	#endregion
	
	#region Private Methods
	
	private void OnToggleSliderValueChanged(bool isOn)
	{
		ManagerData.SETTINGS_SOUND_ON = isOn;
		GameEvents.OnSettingsChanged.Emit(isOn);
	}
	
	#endregion
}