using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CS;
using UnityEngine;

public partial class ManagerGame //_Core
{
	private GameResult _gameResult;
	
	private bool _hasStarted = false;
	private bool _popupOn = false;
	private bool _active = true;
	
	private CancellationTokenSource _cts;
	
	private HashSet<Task> _bulletRunningAssignments   = new();
	private HashSet<Task> _turretRunningAssignments = new();
	
	private bool IsAnyBulletAssignmentRunning => _bulletRunningAssignments.Count > 0;
	private bool IsAnyTurretAssignmentRunning => _turretRunningAssignments.Count > 0;
	private bool isAnyAssignmentRunning       => IsAnyBulletAssignmentRunning || IsAnyTurretAssignmentRunning;

	
	public GameResult GameResult => _gameResult;

	private void Awake_Core()
	{
		_cts = new CancellationTokenSource();
		
		GameEvents.OnLose.SubscribeUntilDestroy(r=> SetGameResult(r), this);
		GameEvents.OnWin.SubscribeUntilDestroy(r=> SetGameResult(r), this);
		GameEvents.OnEnd.SubscribeUntilDestroy(r=> SetGameResult(r), this);
		GameEvents.OnStartPlaying.SubscribeOnceUntilDestroy(OnStartPlaying, this);
		GameEvents.OnTutorialCompleted.SubscribeOnceUntilDestroy(CompleteTutorial, this);
	}
	
	private void Start_Core()
	{
		
	}

	private void Update_Core()
	{
		if (PlayerController && _active && _hasStarted && !_popupOn && _gameResult == GameResult.None)
			PlayerController.CheckMove();
		
		CheckCountdown();
	}
	
	public async void TrackPlayerAssignment(Task task, Action onComplete = null)
	{
		_bulletRunningAssignments.Add(task);
		await task;
		_bulletRunningAssignments.Remove(task);
		onComplete?.Invoke();
	}
	
	public void CancelAllRunningTasks()
	{
		if (_cts == null) return;
		
		_cts.Cancel();
		_cts.Dispose();
		_cts = new CancellationTokenSource();
		_bulletRunningAssignments.Clear();
		_turretRunningAssignments.Clear();
	}
	
	private async void HandleRaycastMechanism(Vector2 position)
	{
		
	}

	Plane plane = new Plane(Vector3.up, Vector3.zero);
	public void HandleRaycastDragMechanism(Vector2 mousePosition)
	{
		if (PlayerController && _active && _hasStarted && !_popupOn)
			PlayerController.PendingNextMove(true);
	}

	public void HandleSwipeMechanism(int directionIndex)
	{
		if(PlayerController && _active && _hasStarted && !_popupOn)
			PlayerController.MoveByInput(directionIndex);
	}
	
	public void HandleRelease()
	{
		if (playerController)
			playerController.PendingNextMove(false);
		
		ClearData();
	}
	
	public void ClearData()
	{
		// isDowning = false;
	}
	
	private void SetGameResult(GameResult result)
	{
		if(_gameResult != GameResult.None)
			return;
		
		ActiveGameStatus(false);
		StopAllControllers();
		_gameResult = result;
		StartCoroutine(Timeline());

		IEnumerator Timeline()
		{
			yield return Yielder.Wait(1);
			switch (_gameResult)
			{
				case GameResult.None: break;
				case GameResult.Win:
					if (ManagerData.MAX_LEVEL_UNLOCKED <= ManagerData.CURRENT_LEVEL_ID)
						ManagerData.UnlockCurrentLevel();
					else
					{
						ManagerData.CURRENT_LEVEL_ID = ManagerData.MAX_LEVEL_UNLOCKED;	
					}
					ManagerSounds.Instance.StopMusic(MusicType.Gameplay);
					ManagerSounds.Instance.PlaySound(SoundType.Victory);
					ManagerUI.Instance.OpenPopup(PopupType.Victory);
					break;
				case GameResult.Lose:
					ManagerSounds.Instance.StopMusic(MusicType.Gameplay);
					ManagerSounds.Instance.PlaySound(SoundType.Lose);
					ManagerUI.Instance.OpenPopup(PopupType.Lose);
					break;
				default:
					break;
			}
		}
	}
	
	private void StopAllControllers()
	{
		playerController.Stop();
	}

	private bool _canCountDown = false;
	Countdowner _countdowner = new Countdowner();
	public void StartCoundown()
	{
		_canCountDown = true;
		_countdowner.StartCd(1);
	}
	private void CheckCountdown()
	{
		if(!_canCountDown) 
			return;
		if (_countdowner.IsCd())
		{
			_countdowner.Cd();
			if (_countdowner.IsOut())
			{
				_currentSecond--;
				ManagerUI.Instance.UpdateTimer(_currentSecond);
		
				if(_currentSecond == 0)
					GameEvents.OnStartPlaying.Emit();
				else
					_countdowner.StartCd(1);
			}
		}
	}

	public void ActiveGameStatus(bool value)
	{
		_active = value;
	}

	public void TogglePopupStatus(bool value)
	{
		_popupOn = value;
	}

	private void OnStartPlaying()
	{
		ActiveGameStatus(true);
		_canCountDown = false;
		_hasStarted = true;
	}
	
	public void StartTutorial()
	{
		ManagerUI.Instance.ToggleOverlay(true);
	}
	
	public void StopAndShowTutorial()
	{
		ActiveGameStatus(false);
		ManagerUI.Instance.ShowTutorial(true);
	}
	
	public void CompleteTutorial()
	{
		ActiveGameStatus(true);
		ManagerUI.Instance.ToggleOverlay(false);
		ManagerUI.Instance.ShowTutorial(false);
		ManagerData.TUTORIAL_COMPLETED = true;
	}
}