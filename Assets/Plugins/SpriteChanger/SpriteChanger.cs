
using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
# endif

namespace UnityEngine.UI
{
	public enum ChangerType { Sprite, Color, }
	[RequireComponent(typeof(Image))]
      public class SpriteChanger : MonoBehaviour
      {
      	#region Inspector Variables
	      
	      [SerializeField] private ChangerType changerType;
	      [SerializeField] private Sprite      onSprite;
	      [SerializeField] private Color       onColor;
	      [SerializeField] private Sprite      offSprite;
	      [SerializeField] private Color       offColor;
      	
      	#endregion
      	
      	#region Member Variables
      	
      	private Image image;
      	
      	#endregion
      	
      	#region Properties
      	
      	#endregion
      	
      	#region Unity Methods
      	
      	private void Awake()
      	{
      		if (image == null) image = GetComponent<Image>();
      	}
      	
      	#endregion
      	
      	#region Public Methods
      	
      	public void SetSprite(bool state)
      	{
		      switch (changerType)
		      {
			      case ChangerType.Sprite: if (image) image.sprite = state ? onSprite : offSprite; break;
			      case ChangerType.Color:  if (image) image.color = state ? onColor : offColor; break;
		      }
      	}
      	
      	#endregion
      	
      	#region Protected Methods
      	
      	#endregion
      	
      	#region Prvate Methods
      	
      	#endregion
      }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpriteChanger), true)]
public sealed class SpriteChangerEditor : Editor
{
	public SerializedProperty changerType;
	public SerializedProperty onSprite;
	public SerializedProperty onColor;
	public SerializedProperty offSprite;
	public SerializedProperty offColor;
	
	private void OnEnable()
	{
		changerType = serializedObject.FindProperty(nameof(changerType));
		onSprite    = serializedObject.FindProperty(nameof(onSprite));
		onColor     = serializedObject.FindProperty(nameof(onColor));
		offSprite   = serializedObject.FindProperty(nameof(offSprite));
		offColor    = serializedObject.FindProperty(nameof(offColor));
	}
	
	public override void OnInspectorGUI()
	{
		var customImage = (SpriteChanger)target;
		if (!customImage) return;
		
		serializedObject.Update();
		DrawValues();
		serializedObject.ApplyModifiedProperties();
	}
	
	private void DrawValues()
	{
		EditorGUILayout.PropertyField(changerType, new GUIContent("Changer Type"));
		
		var type = (ChangerType)changerType.enumValueIndex;
		if (type == ChangerType.Sprite)
		{
			EditorGUILayout.PropertyField(onSprite,  new GUIContent("On Sprite",  "Displayed image when active or in an 'on' state"));
			EditorGUILayout.PropertyField(offSprite, new GUIContent("Off Sprite", "Alternative image when inactive or in an 'off' state"));
		}
		else if (type == ChangerType.Color)
		{
			EditorGUILayout.PropertyField(onColor,  new GUIContent("On Color",  "Displayed color when active or in an 'on' state"));
			EditorGUILayout.PropertyField(offColor, new GUIContent("Off Color", "Alternative color when inactive or in an 'off' state"));
		}
		
		EditorGUILayout.Space();
	}

}
#endif
