
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class ManagerGame //_Level
{
	private int _targetAmount;
	private int _currentAmount;
	private int _currentSecond;
	private int _time;
	private int width, height;
	private bool isInitialized = false;
	private SOLevelConfig currentLevelConfig;
	 
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if(currentLevelConfig != null)
			Destroy(currentLevelConfig);
	}

	private void Awake_Level()
	{
	}
	
	private void Start_Level()
	{
		LoadLevel(ManagerData.CURRENT_LEVEL_ID);
	}
	
	public void RestartLevel()
	{
		// LoadLevel(ManagerData.CURRENT_LEVEL_ID);
		SceneManager.LoadScene(SceneIndexes.Gameplay.ToString());
	}
	
	public void LoadNextLevel()
	{
		ManagerData.CURRENT_LEVEL_ID++;
		SceneManager.LoadScene(SceneIndexes.Gameplay.ToString());
		// LoadLevel(ManagerData.CURRENT_LEVEL_ID);
	}
	
	private void LoadLevel(int levelID)
	{
		Time.timeScale = 1;
		isInitialized  = false;
		CancelAllRunningTasks();
		StopAllControllers();
		SetGameResult(GameResult.None);
		ManagerUI.Instance.CloseAllPopup();
		_currentSecond = 4;
		int totalLevels = levelTable.totalLevelCount;
		int realLevelID = levelID > totalLevels ? ((levelID - 6) % (totalLevels - 5)) + 6 : levelID;
		currentLevelConfig = Instantiate(levelTable.GetLevelConfig(realLevelID));
		
		if (currentLevelConfig)
		{
			_currentAmount = 0;
			
			boardController.Initialize(currentLevelConfig);
			playerController.Initialize();
			
			ManagerUI.Instance.Init();
			
			isInitialized = true;
			
			GameEvents.OnLevelLoaded.Emit();
		}
	}

}