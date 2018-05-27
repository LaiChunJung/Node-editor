using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoModel))]
public class AutoModelEditor : Editor
{

	AutoModel autoModel;
	public void OnEnable()
	{
		autoModel = (AutoModel)target;
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.Label(" AutoModel Ver.1.0 ");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Speed : ");
		autoModel.speed = EditorGUILayout.FloatField(autoModel.speed, GUILayout.Width(50));
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}
}
