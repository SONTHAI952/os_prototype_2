using UnityEngine;

[CreateAssetMenu(fileName = "CellFeelsSettings", menuName = "Scriptable Objects/CellFeelsSettings")]
public class SOCellFeelsSettings : ScriptableObject
{
	[Header("Spacing")]
	[Tooltip("Spacing distance between each cell")]
	public float cellSpacing = 0.65f;
	
	[Header("Speed")]
	[Tooltip("Movement speed of the cell")]
	public float cellMovingSpeed = 1f;
	[Tooltip("Follow speed when the cell tracks another position or object")]
	public float cellFollowingSpeed = 1f;
}