
using UnityEngine;
using UnityEngine.UI;

public class UIPopupVictory : PanelAnimation
{
	#region Inspector Variables
	
	[SerializeField] private Button buttonNext;
	
	#endregion
	
	#region Member Variables
	
	#endregion
	
	#region Properties
	
	#endregion
	
	#region Unity Methods
	
	protected new void Awake()
	{
		base.Awake();
		if (buttonNext) buttonNext.onClick.AddListener(OnButtonNextClick);
	}
	
	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Protected Methods
	
	#endregion
	
	#region Private Methods
	
	private void OnButtonNextClick()
	{
		ManagerGame.Instance.LoadNextLevel();
	}
	
	#endregion
}