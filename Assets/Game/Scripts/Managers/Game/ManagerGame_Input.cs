using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public partial class ManagerGame //_Input
{
	[Header("Input")] 
	[SerializeField] private LayerMask RaycastableTap;
	[SerializeField] private LayerMask RaycastableDrag;
	[SerializeField] float offsetY = 1.5f;
	
	private EventSystem eventSystem;
	private Vector2 touch0StartPosition;
	private Vector2 touch0LastPosition;
	private float touch0StartTime;
	private bool  isTouching;
	
#if UNITY_EDITOR
	private float inputMultiplier = 1f;
#else
		private float inputMultiplier = 5f;
#endif
	
	private void Awake_Input()
	{
		eventSystem = EventSystem.current;
	}
	
	private void Start_Input()
	{
	}
	
	private void Update_Input()
	{
		if (eventSystem.IsPointerOverGameObject()) return;
		if (Input.touchSupported && Input.touchCount > 0) 
			UpdateWithTouch();
		else 
			UpdateWithMouse();
	}
	
	protected void UpdateWithTouch()
	{
		var touchCount = Input.touchCount;
		if (touchCount == 1)
		{
			var touch = Input.GetTouch(0);
			switch (touch.phase)
			{
				//------------------------------------------------------------------------------------------------------
				case TouchPhase.Began:
					//if (isPointerOverUI) return;
					touch0StartPosition = touch.position;
					touch0StartTime     = Time.time;
					touch0LastPosition  = touch0StartPosition;
					isTouching          = true;
					
					OnTouch(touch.position);
					
					OnDrag(touch0LastPosition);
					break;
				//------------------------------------------------------------------------------------------------------
				case TouchPhase.Moved:
					// touch0LastPosition = touch.position;
					
					OnDrag(touch0LastPosition);
					
					// if (touch.deltaPosition != Vector2.zero && isTouching)
					// {
					// 	if (touch.deltaPosition.magnitude > gameFeelsSettings.swipeThreadshole) 
					// 		OnSwipe(touch.deltaPosition);
					// }
					
					break;
				//------------------------------------------------------------------------------------------------------
				case TouchPhase.Stationary: break;
				//------------------------------------------------------------------------------------------------------
				case TouchPhase.Ended:
					if (isTouching)
					{
						if (Time.time - touch0StartTime <= gameFeelsSettings.maxDurationForTap && Vector2.Distance(touch.position, touch0StartPosition) <= gameFeelsSettings.maxDistanceForTap)
						{
							OnClick(touch.position);
						}
						
						touch0LastPosition = touch.position;

						OnDrag(touch0LastPosition);
						if (touch.deltaPosition != Vector2.zero && isTouching)
						{
							if (touch.deltaPosition.magnitude > gameFeelsSettings.swipeThreadshole) 
								OnSwipe(touch.deltaPosition);
						}
						
						isTouching = false;
					}
					
					OnRelease();
					break;
				//------------------------------------------------------------------------------------------------------
				case TouchPhase.Canceled: break;
				//------------------------------------------------------------------------------------------------------
				default: throw new ArgumentOutOfRangeException();
				//------------------------------------------------------------------------------------------------------
			}
		}
		else if (touchCount == 2)
		{
			var touch0 = Input.GetTouch(0);
			var touch1 = Input.GetTouch(1);
			
			if (touch0.phase == TouchPhase.Ended && touch1.phase == TouchPhase.Ended) return;
			
			isTouching = true;
			//canRotateTargetObject = false;
			
			var previousDistance = Vector2.Distance(touch0.position - touch0.deltaPosition, touch1.position - touch1.deltaPosition);
			var currentDistance  = Vector2.Distance(touch0.position,                        touch1.position);
			
			if (!Mathf.Approximately(previousDistance, currentDistance))
			{
				OnPinch(previousDistance, currentDistance);
			}
		}
		else
		{
			if (isTouching)
			{
				isTouching = false;
				//canRotateTargetObject = false;
			}
		}
	}
	
	protected void UpdateWithMouse()
	{
		Vector2 mousePosition = Input.mousePosition;
		
		if (Input.GetMouseButtonDown(0))
		{
			if (isPointerOverUI) return;
			touch0StartPosition = mousePosition;
			touch0StartTime     = Time.time;
			touch0LastPosition  = touch0StartPosition;
			isTouching          = true;
			
			OnTouch(mousePosition);
		}
		
		if (Input.GetMouseButton(0))
		{
			if (isTouching)
			{
				// var delta = mousePosition - touch0LastPosition;
				// touch0LastPosition = mousePosition;
				
				OnDrag(touch0LastPosition);
				
				// if (delta != Vector2.zero && delta.magnitude > gameFeelsSettings.swipeThreadshole) 
				// 	OnSwipe(delta);
			}
		}
		
		if (Input.GetMouseButtonUp(0))
		{
			if (isTouching)
			{
				if (Time.time - touch0StartTime <= gameFeelsSettings.maxDurationForTap && Vector2.Distance(mousePosition, touch0StartPosition) <= gameFeelsSettings.maxDistanceForTap)
				{
					OnClick(mousePosition);
				}
				
				var delta = mousePosition - touch0LastPosition;
				touch0LastPosition = mousePosition;
				
				OnDrag(touch0LastPosition);
				
				if (delta != Vector2.zero && delta.magnitude > gameFeelsSettings.swipeThreadshole) 
					OnSwipe(delta);
				
				isTouching = false;
			}
			
			OnRelease();
		}
		
		if (Input.mouseScrollDelta.y != 0)
		{
			OnPinch(1, Input.mouseScrollDelta.y < 0 ? (1 / gameFeelsSettings.mouseScrollSpeed) : gameFeelsSettings.mouseScrollSpeed);
		}
	}
	
	/// <summary>
	/// fhfghhfhhffh
	/// </summary>
	/// <param name="position"></param>
	private void OnTouch(Vector2 position)
	{
		if (!TryPhysicsRaycast(mainCamera.ScreenPointToRay(position), out var raycastHit)) 
			return;
		
		HandleRaycastMechanism(position);
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="position"></param>
	private void OnClick(Vector2 position)
	{
		if (!TryPhysicsRaycast(mainCamera.ScreenPointToRay(position), out var raycastHit)) 
			return;
		
		HandleRaycastMechanism(position);
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="deltaPosition"></param>
	private void OnSwipe(Vector2 delta)
	{
		// Nếu vuốt theo trục Y nhiều hơn trục X → bỏ qua
		if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
			return;

		// Detect trái / phải
		if (delta.x > 0)
		{
			HandleSwipeMechanism(2);
		}
		else
		{
			HandleSwipeMechanism(1);
		}
	}


	private void OnDrag(Vector2 mousePosition)
	{
		HandleRaycastDragMechanism(mousePosition);
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="oldDistance"></param>
	/// <param name="newDistance"></param>
	private void OnPinch(float oldDistance, float newDistance)
	{
	}
	
	/// <summary>
	/// 
	/// </summary>
	private void OnRelease()
	{
		HandleRelease();
	}
	
	
	private bool TryPhysicsRaycast(Ray ray, out RaycastHit hit)
	{
		return Physics.Raycast(ray, out hit, 200f, RaycastableTap);
	}
}