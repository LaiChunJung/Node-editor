using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIRig))]
public class AIRigEditor : Editor
{

	private AIRig mAiRig;
	public void OnEnable()
	{
		mAiRig = (AIRig)target;
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.Label(" AI　Rig ");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Asset : ");
		
		mAiRig.AIData = (NodeSystem.CreatNodeObject)EditorGUILayout.ObjectField((mAiRig.AIData), typeof(object), false, GUILayout.Width(300f));
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}
}
