//********************************************************************************
// Name spaces
//********************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using NodeSystem;


namespace NodeSystem
{
	public class ActionNode : NodeBase
	{
		public enum ActionType
		{
			DIALOG,
			PATROL
		}

		public NodeConnectionPoint InPoint;
		public List<NodeConnectionPoint> OutPoints = new List<NodeConnectionPoint>();
		public List<string> Triggers = new List<string>();
		public List<Vector3> TargetPosition = new List<Vector3>();

		public string Name;
		public ActionType NodeActionType;
		private float mOffset;
		private float mButton_height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 4f;
		private bool isAddClicked = false;
		private bool isRemoveClicked = false;
		private Vector2 scroll;

		public ActionNode(DialogSystemEditor editor, Vector2 position) : base(editor, position)
		{
			Init(position);
		}

		public ActionNode(DialogSystemEditor editor, NodeData iData) : base(editor, iData)
		{
			Init(new Vector2(iData.NodeRect.x, iData.NodeRect.y));
			SetTriggers(iData.Triggers);
			Name = iData.NodeName;
		}

		public override void Init(Vector2 position)
		{
			Width = 300;
			Height = 200;
			NodeRect = new Rect(position.x, position.y, Width, Height);
			InPoint = new NodeConnectionPoint(this, NodeConnectionPointType.In, Editor.OnClickInPoint);
			OutPoints.Add(new NodeConnectionPoint(this, NodeConnectionPointType.Out, Editor.OnClickOutPoint));
			Triggers.Add("default");
			TargetPosition.Add(Vector3.zero);
		}

		public void SetTriggers(List<string> iTriggers)
		{
			this.Triggers = iTriggers;
		}

		public override void Draw()
		{
			NodeRect.height = mOffset + ((3 + Triggers.Count+TargetPosition.Count) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing)) + 10 + mButton_height + (EditorGUIUtility.singleLineHeight * 5);

			InPoint.Draw();

			for (int i = Triggers.Count - 1; i >= 0; i--)
			{
				OutPoints[i].Draw(NodeRect.y + mOffset + ((2 + i) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing)) - (OutPoints[i].ConnectionPointRect.height * 0.5f) + (EditorGUIUtility.singleLineHeight * 0.5f) + mButton_height);
			}

			GUI.Box(NodeRect, Name, Style);

			GUILayout.BeginArea(new Rect(NodeRect.x, NodeRect.y + mOffset, NodeRect.width, NodeRect.height - mOffset));
			GUILayout.BeginVertical();

			Name = EditorGUILayout.TextField("Name", Name);
			Name = "Init_node";
			GUILayout.BeginHorizontal();
			isRemoveClicked = GUILayout.Button("-");
			isAddClicked = GUILayout.Button("+");
			GUILayout.EndHorizontal();
			NodeActionType = (ActionType)EditorGUILayout.EnumPopup("Node Action Type", NodeActionType);
			switch (NodeActionType)
			{
				case ActionType.DIALOG:
					for (int i = 0; i < Triggers.Count; i++)
					{
						Triggers[i] = EditorGUILayout.TextField("Option " + i, Triggers[i]);
					}
					break;
				case ActionType.PATROL:
					for (int i = 0; i < TargetPosition.Count; i++)
					{
						TargetPosition[i] = EditorGUILayout.Vector3Field("TargetPosition " + i, TargetPosition[i]);
					}
					break;
				default:
					break;
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		public override bool ProcessEvents(Event iEvent)
		{
			ProcessDefault(iEvent);
			InPoint.ProcessEvents(iEvent);
			for (int i = 0; i < OutPoints.Count; i++)
			{
				OutPoints[i].ProcessEvents(iEvent);
			}
			if (isAddClicked)
			{
				switch (NodeActionType)
				{
					case ActionType.DIALOG:
						OutPoints.Add(new NodeConnectionPoint(this, NodeConnectionPointType.Out, Editor.OnClickOutPoint));
						Triggers.Add("");
						break;
					case ActionType.PATROL:
						TargetPosition.Add(Vector3.zero);
						break;
					default:
						break;
				}
			}
			else if (isRemoveClicked && OutPoints.Count > 1)
			{
				switch (NodeActionType)
				{
					case ActionType.DIALOG:
						OutPoints.RemoveAt(OutPoints.Count - 1);
						Triggers.RemoveAt(Triggers.Count - 1);
						break;
					case ActionType.PATROL:
						TargetPosition.RemoveAt(Triggers.Count - 1);
						break;
					default:
						break;
				}
			}
			return false;
		}

		public override void SetStyle()
		{
			Style.normal.background = AssetDatabase.LoadAssetAtPath("Assets/Editor/DialogNodeEditor/Textures/grayTex.png", typeof(Texture2D)) as Texture2D;
			Style.onFocused.background = AssetDatabase.LoadAssetAtPath("Assets/Editor/DialogNodeEditor/Textures/grayDarkTex.png", typeof(Texture2D)) as Texture2D;
			Style.normal.textColor = Color.white;
			Style.fontSize = 16;
			Style.alignment = TextAnchor.UpperCenter;

			GUIContent content = new GUIContent(Name);
			mOffset = Style.CalcSize(content).y;
		}

		public override List<NodeConnectionPoint> GetConnectionPoints()
		{
			List<NodeConnectionPoint> result = new List<NodeConnectionPoint> { InPoint };
			result.AddRange(OutPoints);
			return result;
		}

		public ActionNode Clone()
		{
			return (ActionNode)MemberwiseClone();
		}

		public override NodeData GetInfo()
		{
			return new NodeData(GetType().FullName, NodeRect, Name, Triggers ,TargetPosition);
		}

		public override void Rebuild(List<NodeConnectionPoint> iConnectionPoint)
		{
			InPoint = iConnectionPoint[0];
			OutPoints = iConnectionPoint.GetRange(1, iConnectionPoint.Count - 1);

			InPoint.Rebuild(this, NodeConnectionPointType.In, Editor.OnClickInPoint);
			for (int i = 0; i < OutPoints.Count; i++)
			{
				OutPoints[i].Rebuild(this, NodeConnectionPointType.Out, Editor.OnClickOutPoint);
			}
		}
	}
}