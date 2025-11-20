using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public partial class ManagerGame //_Core
{
	private GameResult _gameResult;
	
	private bool _active = true;
	
	private CancellationTokenSource _cts;
	
	private HashSet<Task> _bulletRunningAssignments   = new();
	private HashSet<Task> _turretRunningAssignments = new();
	
	private bool IsAnyBulletAssignmentRunning => _bulletRunningAssignments.Count > 0;
	private bool IsAnyTurretAssignmentRunning => _turretRunningAssignments.Count > 0;
	private bool isAnyAssignmentRunning       => IsAnyBulletAssignmentRunning || IsAnyTurretAssignmentRunning;


	private void Awake_Core()
	{
		_cts = new CancellationTokenSource();
		
		GameEvents.OnOutOfSpace.SubscribeUntilDestroy(r=> SetGameResult(r), this);
		GameEvents.OnOutOfTime.SubscribeUntilDestroy(r=> SetGameResult(r), this);
		GameEvents.OnTargetComplete.SubscribeUntilDestroy(r=> SetGameResult(r), this);
		GameEvents.OnStartPlaying.SubscribeOnceUntilDestroy(StartCoundown,this);
	}
	
	private void Start_Core()
	{
		
	}

	private void Update_Core()
	{
		CheckCountdown();
	}
	
	public async void TrackBulletAssignment(Task task, Action onComplete = null)
	{
		_bulletRunningAssignments.Add(task);
		await task;
		_bulletRunningAssignments.Remove(task);
		onComplete?.Invoke();
	}
	
	public async void TrackTurretAssignment(Task task, Action onComplete = null)
	{
		_turretRunningAssignments.Add(task);
		await task;
		_turretRunningAssignments.Remove(task);
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
	
	private Tube _selectingTube;
	private async void HandleRaycastMechanism(Vector2 position)
	{
		if (!_canCountDown)
			GameEvents.OnStartPlaying.Emit();
		
		if (!TryPhysicsRaycast(mainCamera.ScreenPointToRay(position), out var raycastHit)) return;
		var tube = raycastHit.collider.GetComponent<Tube>();
		if(tube.PickUpable)
			_selectingTube = tube;
	}

	Plane plane = new Plane(Vector3.up, Vector3.zero);
	private void HandleRaycastDragMechanism(Vector2 mousePosition)
	{
		if(_selectingTube == null) 
			return;
		
		OnMoveTube(mousePosition);
	}
	
	private void OnMoveTube(Vector2 mousePosition)
	{
		var ray = mainCamera.ScreenPointToRay(mousePosition);
		plane.Raycast(ray, out float distance);
		Vector3 pos = ray.GetPoint(distance);
		pos.y += offsetY;
		_selectingTube.transform.position = pos;
	}
	
	private void HandlePutDownTube()
	{
		
		ClearData();
	}
	
	public void ClearData()
	{
		
		// isDowning = false;
	}
	
	private bool isTrackingGameResult;
	private void TrackingGameResult()
	{
		if (isTrackingGameResult) return;
		// trackingGameResultCoroutine.StopIfNotNull(this);
		// trackingGameResultCoroutine = StartCoroutine(IETrackingGameResult());
	}
	
	private Coroutine trackingGameResultCoroutine;
	// private IEnumerator IETrackingGameResult()
	// {
	// 	isTrackingGameResult = true;
	// 	while (isTrackingGameResult)
	// 	{
	// 		if (boardController.isAllCellCleared && turretController.isAllTurretCleared)
	// 		{
	// 			SetGameResult(GameResult.Win);
	// 			isTrackingGameResult = false;
	// 			yield break;
	// 		}
	// 		
	// 		if (boardController.isCellHeadReachedEndPoint && !isAnyAssignmentRunning)
	// 		{
	// 			SetGameResult(GameResult.Lose);
	// 			isTrackingGameResult = false;
	// 			yield break;
	// 		}
	// 		
	// 		yield return null;
	// 	}
	// }
	
	private void SetGameResult(GameResult result)
	{
		if(_gameResult != GameResult.None)
			return;
		
		_gameResult = result;
		switch (_gameResult)
		{
			case GameResult.None: break;
			case GameResult.Win:
				ManagerUI.Instance.OpenPopup(PopupType.Victory);
				ManagerSounds.Instance.PlaySound(SoundType.Victory);
				StopAllControllers();
				break;
			case GameResult.Lose:
				ManagerUI.Instance.OpenPopup(PopupType.Lose);
				ManagerSounds.Instance.PlaySound(SoundType.Lose);
				StopAllControllers();
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}
	
	private void StopAllControllers()
	{
		
	}

	private bool _canCountDown = false;
	Countdowner _countdowner = new Countdowner();
	void StartCoundown()
	{
		_canCountDown = true;
		_countdowner.StartCd(1);
	}
	
	private void CheckCountdown()
	{
		if(!_canCountDown || !_active) 
			return;

		if (_countdowner.IsCd())
		{
			_countdowner.Cd();
			if (_countdowner.IsOut())
			{
				_currentSecond--;
				ManagerUI.Instance.UpdateTimer(_currentSecond);
		
				if(_currentSecond == 0)
					GameEvents.OnOutOfTime.Emit(GameResult.Lose);
				else
					_countdowner.StartCd(1);
			}
		}
	}

	public void ActiveGameStatus(bool value)
	{
		_active = value;
	}
}