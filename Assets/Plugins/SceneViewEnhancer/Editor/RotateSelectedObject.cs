using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class RotateSelectedObject
{
	static RotateSelectedObject()
	{
		SceneView.duringSceneGui += OnSceneGUI;
	}
	
	private static void OnSceneGUI(SceneView sceneView)
	{
		Event e = Event.current;
		
		if (e.type == EventType.KeyDown && e.shift && e.keyCode == KeyCode.V)
		{
			RotateSelected();
			e.Use();
		}
	}
	
	private static void RotateSelected()
	{
		foreach (GameObject obj in Selection.gameObjects)
		{
			Undo.RecordObject(obj.transform, "Rotate 90 Degrees");
			obj.transform.Rotate(Vector3.up, 90f, Space.World);
		}
		
		SceneView.RepaintAll();
	}
}