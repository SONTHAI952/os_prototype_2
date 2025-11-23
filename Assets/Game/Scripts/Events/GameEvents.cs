using UnityEngine;
using ZeroX.RxSystem;

public static class GameEvents
{
	public static readonly Subject OnLevelLoaded = new();
	public static readonly Subject<bool> OnSettingsChanged = new();
	
	
	public static readonly Subject<GameResult> OnWin      = new();
	public static readonly Subject<GameResult> OnLose      = new();
	public static readonly Subject OnStartPlaying = new();
}