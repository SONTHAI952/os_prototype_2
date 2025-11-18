using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
# endif

/// ====================================================================================
///		? 2023 Turongson. All rights reserved. (TM)
/// 
///		Description: Lightweight Unity button script, designed as a flexible replacement 
///		for the  traditional Button component. Offers customizable sprites and optional 
///		animations for interactive UI elements.
/// 
///		Usage	: Attach this button to a Unity UI GameObject to enable interactive 
///               button functionality. Customize the appearance and behavior using
///               the provided properties in the Unity Editor.
/// 
///		Author	: turongson
///		Contact	: turongson@gmail.com
/// ====================================================================================

namespace UnityEngine.UI
{
	[RequireComponent(typeof(Image))]
	public class ButtonLite : MonoBehaviour, IPointerClickHandler
	{
		#region Inspector Variables
		
		public bool       interactable  = true;
		public bool       animateable   = true;
		public bool       transitenable = false;
		public Image      image;
		public Sprite     onSprite;
		public Color      onColor;
		public Sprite     offSprite;
		public Color      offColor;
		public UnityEvent onClick;
		
		#endregion
		
		#region Member Variables
		
		private Vector3 originalScale;
		private Vector3 targetScale;
		private float   scaleDuration   = 0.2f;
		private float   scaleMultiplier = 1.2f;
		private float   elapsedTime     = 0f;
		private float   halfScaleTime   = 0f;
		
		#endregion
		
		#region Properties
		
		public bool SpriteState { get; set; }
		
		#endregion
		
		#region Unity Methods
		
#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			SetupImage();
		}
#endif
		
		private void Awake()
		{
			SetupImage();
			SetupButton();
		}

		private void OnEnable()
		{
			SetLocalScale(originalScale);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!interactable) return;
			if (animateable) PunchScale();
			onClick?.Invoke();
		}
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="call"></param>
		public void AddListener(UnityAction call)
		{
			onClick.AddListener(call);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		/// <param name="animate"></param>
		public void SetSprite(bool state, bool animate = false, bool isSetNativeSize = false)
		{
			if (!transitenable) return;
			if (image)
			{
				SpriteState  = state;
				if (onSprite && offSprite)
					image.sprite = state ? onSprite : offSprite;
				else 
					image.color = state ? onColor : offColor;
				if (isSetNativeSize) image.SetNativeSize();
				if (animate) PunchScale();
			}
		}
		
		public void Click()
		{
			onClick?.Invoke();
		}
		
		#endregion
		
		#region Private Methods
		
		private void SetupImage()
		{
			if (!image)
			{
				image       = GetComponent<Image>();
				SpriteState = (image.sprite == onSprite);
			}
		}
		
		private void SetupButton()
		{
			originalScale = transform.localScale;
			halfScaleTime = scaleDuration * 0.5f;
			targetScale   = originalScale * scaleMultiplier;
		}
		
		public void PunchScale()
		{
			StopAllCoroutines();
			StartCoroutine(IEPunchScale());
		}
		
		private void SetLocalScale(Vector3 targetScale)
		{
			transform.localScale = targetScale;
		}
		
		private IEnumerator IEPunchScale()
		{
			SetLocalScale(originalScale);
			
			elapsedTime = 0f;
			while (elapsedTime < halfScaleTime)
			{
				elapsedTime += Time.deltaTime;
				SetLocalScale(Vector3.Lerp(originalScale, targetScale, elapsedTime / halfScaleTime));
				yield return null;
			}
			
			elapsedTime = 0f;
			while (elapsedTime < halfScaleTime)
			{
				elapsedTime += Time.deltaTime;
				SetLocalScale(Vector3.Lerp(targetScale, originalScale, elapsedTime / halfScaleTime));
				yield return null;
			}
			
			SetLocalScale(originalScale);
		}
		
		#endregion
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonLite), true)]
public sealed class ButtonLiteEditor : Editor
{
	public SerializedProperty interactable;
	public SerializedProperty animateable;
	public SerializedProperty transitenable;
	public SerializedProperty onSprite;
	public SerializedProperty onColor;
	public SerializedProperty offSprite;
	public SerializedProperty offColor;
	public SerializedProperty onClick;
	
	private void OnEnable()
	{
		interactable  = serializedObject.FindProperty(nameof(interactable));
		animateable   = serializedObject.FindProperty(nameof(animateable));
		transitenable = serializedObject.FindProperty(nameof(transitenable));
		onSprite      = serializedObject.FindProperty(nameof(onSprite));
		onColor       = serializedObject.FindProperty(nameof(onColor));
		offSprite     = serializedObject.FindProperty(nameof(offSprite));
		offColor       = serializedObject.FindProperty(nameof(offColor));
		onClick       = serializedObject.FindProperty(nameof(onClick));
	}
	
	public override void OnInspectorGUI()
	{
		var customButton = (ButtonLite)target;
		if (!customButton) return;
		
		serializedObject.Update();
		DrawValues();
		serializedObject.ApplyModifiedProperties();
	}
	
	private void DrawValues()
	{
		EditorGUILayout.PropertyField(interactable,  new GUIContent("Interactable",  "Can the Selectable be interacted with?"));
		EditorGUILayout.PropertyField(animateable,   new GUIContent("Animateable",   "Enables interactive animation"));
		EditorGUILayout.PropertyField(transitenable, new GUIContent("Transitenable", "Enables sprite transition effects"));

		if (transitenable.boolValue)
		{
			EditorGUILayout.PropertyField(onSprite,  new GUIContent("On Sprite",  "Displayed image when active or in an 'on' state"));
			EditorGUILayout.PropertyField(onColor,   new GUIContent("On Color",  "Displayed color when active or in an 'on' state"));
			EditorGUILayout.PropertyField(offSprite, new GUIContent("Off Sprite", "Alternative image when inactive or in an 'off' state"));
			EditorGUILayout.PropertyField(offColor, new GUIContent("Off Color", "Alternative color when inactive or in an 'off' state"));
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(onClick, new GUIContent("On Click"));
	}
}
#endif