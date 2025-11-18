	
using UnityEngine;
using UnityEngine.UI;

public class UIPopupLose : PanelAnimation
{
	#region Inspector Variables
	
	[SerializeField] private Button buttonTryNext;
	
	#endregion
	
	#region Member Variables
	
	#endregion
	
	#region Properties
	
	#endregion
	
	#region Unity Methods
	
	protected new void Awake()
	{
		base.Awake();
		if (buttonTryNext) buttonTryNext.onClick.AddListener(OnButtonNextClick);
	}
	
	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Protected Methods
	
	#endregion
	
	#region Private Methods
	
	private void OnButtonNextClick()
	{
		ManagerGame.Instance.RestartLevel();
	}
	
	#endregion
}