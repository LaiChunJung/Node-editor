using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(AIRig))]
public class AIRigEditor : Editor
{

	private AIRig mAiRig;
	private bool mShowAsset = true;
	private bool mShowPath  = true;
	private Texture ButtonTexture;
	[SerializeField]
	private List<Transform> PathNodes = new List<Transform>();

	public void OnEnable()
	{
		mAiRig = (AIRig)target;
		PathNodes = new List<Transform>();
		ButtonTexture = AssetDatabase.LoadAssetAtPath("Assets/Resources/Textures/grayTex.png", typeof(Texture2D)) as Texture2D;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		GUILayout.BeginVertical();
		GUILayout.Label(" Is  Use Asset");

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("AI", GUILayout.Width(30), GUILayout.Height(30)))
		{
			mShowAsset = !mShowAsset;
		}
		if (GUILayout.Button("PA", GUILayout.Width(30), GUILayout.Height(30)))
		{
			mShowPath = !mShowPath;
		}
		GUILayout.EndHorizontal();

		if (mShowAsset)
		{
			GUILayout.BeginVertical();
			GUILayout.Label(" AI_Rig ");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Asset : ");
			mAiRig.AIData = (NodeSystem.CreatNodeObject)EditorGUILayout.ObjectField((mAiRig.AIData), typeof(object), false, GUILayout.Width(200f));
			GUILayout.EndVertical();
		}
		if(mShowPath)
		{
			GUILayout.BeginVertical();
			GUILayout.Label(" Node Path ");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Path : ");
			EditorGUIUtility.LookLikeInspector();
			SerializedProperty PathNodes = serializedObject.FindProperty("PathNodes");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(PathNodes, true);
			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
			EditorGUIUtility.LookLikeControls();
			GUILayout.EndVertical();
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	public void OnDrawGizmosSelected()
	{
		if (mAiRig.PathNodes.Count > 0)
		{
			Gizmos.color = Color.red;
			Vector3 aHeight = Vector3.up * 0.5f;
			for (int i = 0; i < mAiRig.PathNodes.Count; i++)
			{
				Gizmos.DrawSphere(mAiRig.PathNodes[i].position + aHeight, 0.25f);
				if (i == 0)
				{
					Gizmos.DrawLine(mAiRig.transform.position + aHeight, mAiRig.PathNodes[i].position + aHeight);
				}
				else
				{
					Gizmos.DrawLine(mAiRig.PathNodes[i - 1].position + aHeight, mAiRig.PathNodes[i].position + aHeight);
				}
			}
		}
	}
}
