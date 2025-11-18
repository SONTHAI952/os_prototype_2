using UnityEngine;

[CreateAssetMenu(fileName = "GameFeelsSettings", menuName = "Scriptable Objects/GameFeelsSettings")]
public class SOGameFeelsSettings : ScriptableObject
{
	[Header("Input Settings")]
	[Tooltip("Scrolling speed when using the middle mouse button")]
	public float mouseScrollSpeed = 2;
	[Tooltip("Minimum distance required to trigger a swipe gesture")]
	public float swipeThreadshole = 20f;
	
	[Header("Tap Detection")]
	[Tooltip("Maximum movement distance still considered a tap")]
	public float maxDistanceForTap = 20;
	[Tooltip("Maximum press duration still considered a tap")]
	public float maxDurationForTap = 0.1f;
}