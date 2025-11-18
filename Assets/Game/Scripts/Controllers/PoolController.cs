using GDTools.ObjectPooling;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolController : MonoBehaviour
{
	#region Inspector Variables
	
	[SerializeField] private Pool ballPool;
	[SerializeField] private Pool tubePool;
	[SerializeField] private Pool wallPool;
	
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
		ballPool.DestroyAllObjects();
		tubePool.DestroyAllObjects();
	}
	
	public Ball InstantiateBall(Vector3 position, int colorID)
	{
		var ball =  PoolObjectBall(position).GetComponent<Ball>();
		ball.Initialize(colorID);
		return ball;
	}
	
	public void DeInstantiateBall(Ball ball)
	{
		ballPool.DestroyObject(ball.poolObject);
		ball.transform.SetParent(ballPool.transform);
	}
	
	public Tube InstantiateTube(Vector3 position)
	{
		return PoolObjectTube(position).GetComponent<Tube>();
	}
	
	public void DeInstantiateTube(Tube tube)
	{
		tubePool.DestroyObject(tube.PoolObject);
	}
	
	public Wall InstantiateWall(Vector3 position)
	{
		return PoolObjectWall(position).GetComponent<Wall>();
	}
	
	public void DeInstantiateWall(Wall wall)
	{
		wallPool.DestroyObject(wall.PoolObject);
	}
	
	private PoolObject PoolObjectBall(Vector3 position)
	{
		return ballPool.InstantiateObject(position);
	}

	private PoolObject PoolObjectTube(Vector3 position)
	{
		return tubePool.InstantiateObject(position);
	}
	
	private PoolObject PoolObjectWall(Vector3 position)
	{
		return wallPool.InstantiateObject(position);
	}
	
	/*private PoolObject PoolObjectEffect(Vector3 position)
	{
		return effectPool.InstantiateObject(position);
	}
	
	private PoolObject PoolObjectRocket(Vector3 position)
	{
		return rocketPool.InstantiateObject(position);
	}*/
	
	#endregion
}