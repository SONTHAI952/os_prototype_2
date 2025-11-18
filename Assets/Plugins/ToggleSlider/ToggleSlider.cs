using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ToggleSlider : MonoBehaviour
{
	#region Inspector Variables
	
	[SerializeField] private Slider slider;
	[SerializeField] private GameObject onObject;
	[SerializeField] private GameObject offObject;
	
	#endregion
	
	#region Member Variables
	
	
	#endregion
	
	#region Properties
	
	public Action<bool> OnValueChanged;
	
	#endregion
	
	#region Unity Methods
	
	private void Awake()
	{
		slider.onValueChanged.AddListener(OnSliderValueChanged);
	}
	
	#endregion
	
	#region Public Methods
	
	public void SetToggle(bool isOn)
	{
		slider.value = isOn ? 1 : 0;
	}
	
	public bool GetToggle()
	{
		return slider.value != 0;
	}
	
	#endregion
	
	#region Protected Methods
	
	#endregion
	
	#region Private Methods
	
	private void OnSliderValueChanged(float value)
	{
		var isOn = slider.maxValue == (int)value;
		Debug.Log("isOn = " + isOn);
		onObject.SetActive(isOn);
		offObject.SetActive(!isOn);
		OnValueChanged?.Invoke(isOn);
	}
	
	#endregion
}
