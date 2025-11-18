using UnityEngine;

[CreateAssetMenu(fileName = "TurretFeelsSettings", menuName = "Scriptable Objects/TurretFeelsSettings")]
public class SOTurretFeelsSettings : ScriptableObject
{
	[Header("Debug")]
	[Tooltip("Draws the turret's movement path in the Scene view")]
	public bool drawPath;
	
	[Header("Animation")]
	[Tooltip("Animation curve that defines the turret’s shooting motion")]
	public AnimationCurve animationCurve;
	[Tooltip("Extra scale amount added during animation")]
	public float extraScale = 0.2f;
	
	[Header("Duration")]
	[Tooltip("Time it takes for the turret to reach its target position")]
	public float turretMovingDuration = 0.5f;
	[Tooltip("Time it takes for the turret to complete firing one bullet")]
	public float turretFiringDuration = 0.2f;
	[Tooltip("Delay before the turret fires the next bullet")]
	public float turretDelayPerShotDuration = 0.5f;
}