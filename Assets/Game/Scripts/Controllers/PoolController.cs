using GDTools.ObjectPooling;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolController : MonoBehaviour
{
	#region Inspector Variables
	
	// [SerializeField] private Pool ballPool;
	// [SerializeField] private Pool tubePool;
	// [SerializeField] private Pool wallPool;
	
	#endregion
	
	#region Member Variables
	
	#endregion
	
	#region Properties
	
	#endregion
	
	#region Unity Methods
	
	protected void OnAwake()
	{
	}
	
	#endregion
	
	#region Public Methods
	
	/*public Effect InstantiateEffect(Vector3 position, int groupId)
	{
		var effect = PoolObjectEffect(position).GetComponent<Effect>();
		effect.ChangeColor(groupId);
		return effect;
	}
	
	public void DeInstantiateEffect(Effect effect)
	{
		effectPool.DestroyObject(effect.PoolObject);
	}*/
	
	public void Initialize()
	{
		// ballPool.DestroyAllObjects();
		// tubePool.DestroyAllObjects();
	}
	
	// public Ball InstantiateBall(Vector3 position, int colorID)
	// {
	// 	var ball =  PoolObjectBall(position).GetComponent<Ball>();
	// 	ball.Initialize(colorID);
	// 	return ball;
	// }
	//
	// public void DeInstantiateBall(Ball ball)
	// {
	// 	ballPool.DestroyObject(ball.poolObject);
	// 	ball.transform.SetParent(ballPool.transform);
	// }
	
	//
	// private PoolObject PoolObjectBall(Vector3 position)
	// {
	// 	return ballPool.InstantiateObject(position);
	// }

	
	#endregion
}