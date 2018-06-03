using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace NodeSystem
{
	public enum NodeConnectionPointType { In, Out }

	public class NodeConnectionPoint
	{
		public Rect ConnectionPointRect;
		public NodeBase Node;
		public NodeConnectionPointType Type;
		public GUIStyle style = new GUIStyle();

		public Action<NodeConnectionPoint> OnClickConnectionPoint;

		public bool isClicked = false;

		public NodeConnectionPoint(NodeBase iNode, NodeConnectionPointType iType, Action<NodeConnectionPoint> OnClickConnectionPoint)
		{
			this.Node = iNode;
			this.Type = iType;
			this.OnClickConnectionPoint = OnClickConnectionPoint;
			SetStyle();
			ConnectionPointRect = new Rect(0, 0, 10f, 10f);
		}

		public NodeConnectionPoint()
		{
			//for build purposes only
			SetStyle();
			ConnectionPointRect = new Rect(0, 0, 10f, 10f);
		}

		public void ProcessEvents(Event e)
		{
			if (isClicked)
			{
				if (OnClickConnectionPoint != null)
				{
					OnClickConnectionPoint(this);
				}
			}
		}

		public void Draw(float y)
		{
			ConnectionPointRect.y = y;

			switch (Type)
			{
				case NodeConnectionPointType.In:
					ConnectionPointRect.x = Node.NodeRect.x - ConnectionPointRect.width + 0f;
					break;
				case NodeConnectionPointType.Out:
					ConnectionPointRect.x = Node.NodeRect.x + Node.NodeRect.width - 0f;
					break;
			}

			isClicked = GUI.Button(ConnectionPointRect, "", style);
		}

		public void Draw()
		{
			Draw(Node.NodeRect.y + (Node.NodeRect.height * 0.5f) - (ConnectionPointRect.height * 0.5f));
		}

		public void SetStyle()
		{
		}

		public void Rebuild(NodeBase iNode, NodeConnectionPointType iType, Action<NodeConnectionPoint> OnClickConnectionPoint)
		{
			this.Node = iNode;
			this.Type = iType;
			this.OnClickConnectionPoint = OnClickConnectionPoint;
		}
	}
}