using UnityEngine;
using System.Collections;
using UnityEditor;

public class ModelCreater : EditorWindow
{
	void OnGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.Label(" AutoModel Ver.1.0 ");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Create Yuzu ");
		if(GUILayout.Button("Create", GUILayout.Width(50)))
		{
			testFunction();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void testFunction()
	{
		Debug.Log("Fuck");
	}
}