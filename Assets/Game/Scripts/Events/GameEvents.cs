using UnityEngine;
using ZeroX.RxSystem;

public static class GameEvents
{
	public static readonly Subject OnLevelLoaded = new();
	public static readonly Subject<bool> OnSettingsChanged = new();
	
	
	public static readonly Subject<GameResult> OnTargetComplete      = new();
	public static readonly Subject<GameResult> OnOutOfSpace      = new();
	public static readonly Subject<GameResult> OnOutOfTime      = new();
	public static readonly Subject OnStartPlaying = new();
	public static readonly Subject OnBallCollect = new();
}