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
	public class InitNode :NodeBase
	{
		public NodeConnectionPoint OutputPoint;

		public InitNode(DialogSystemEditor editor, Vector2 position) : base(editor, position) {
			Init(position);
		}

		public InitNode(DialogSystemEditor editor, NodeData info) : base(editor, info) {
			Init(new Vector2(info.NodeRect.x, info.NodeRect.y));
		}

		public override void Init(Vector2 position) {
			Width = 200;
			Height = 100;
			NodeRect = new Rect(position.x, position.y, Width, Height);
			OutputPoint = new NodeConnectionPoint(this, NodeConnectionPointType.Out, Editor.OnClickOutPoint);
		}

		public override void Draw() {
			OutputPoint.Draw();

			GUI.Box(NodeRect, "", Style);
			GUI.Label(NodeRect, "Init Node", Style);
		}

		public override bool ProcessEvents(Event e) {
			ProcessDefault(e);
			OutputPoint.ProcessEvents(e);
			return false;
		}

		public override void SetStyle() {
			Style.normal.background = AssetDatabase.LoadAssetAtPath("Assets/Editor/DialogNodeEditor/Textures/grayTex.png", typeof(Texture2D)) as Texture2D;
			Style.onFocused.background = AssetDatabase.LoadAssetAtPath("Assets/Editor/DialogNodeEditor/Textures/grayDarkTex.png", typeof(Texture2D)) as Texture2D;
			Style.normal.textColor = Color.white;
			Style.fontSize = 32;
			Style.alignment = TextAnchor.MiddleCenter;
		}

		public override List<NodeConnectionPoint> GetConnectionPoints() {
			return new List<NodeConnectionPoint> { OutputPoint };
		}

		public override NodeData GetInfo() {
			return new NodeData(GetType().FullName, NodeRect);
		}

		public override void Rebuild(List<NodeConnectionPoint> iConnectionPoint) {
			OutputPoint = iConnectionPoint[0];
			OutputPoint.Rebuild(this, NodeConnectionPointType.Out, Editor.OnClickOutPoint);
		}
	}
}