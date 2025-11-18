
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupRestart : PanelAnimation
{
	#region Inspector Variables
	
	[SerializeField] private Button buttonYes;
	
	#endregion
	
	#region Member Variables
	
	#endregion
	
	#region Properties
	
	#endregion
	
	#region Unity Methods
	
	protected new void Awake()
	{
		base.Awake();
		if (buttonYes) buttonYes.onClick.AddListener(OnButtonYesClick);
		onPanelOpenAction = () => ManagerGame.Instance.ActiveGameStatus(false);
		onPanelCloseAction = () => ManagerGame.Instance.ActiveGameStatus(true);
	}
	
	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Protected Methods
	
	#endregion
	
	#region Private Methods
	
	private void OnButtonYesClick()
	{
		ManagerGame.Instance.RestartLevel();
	}
	
	#endregion
}
