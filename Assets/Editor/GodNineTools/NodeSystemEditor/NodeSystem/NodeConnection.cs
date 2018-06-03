using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace NodeSystem
{
	public class NodeConnection
	{
		public NodeConnectionPoint InPoint;
		public NodeConnectionPoint OutPoint;

		public Action<NodeConnection> OnClickRemoveConnection;

		public bool isClicked = false;

		public NodeConnection(NodeConnectionPoint iInPoint, NodeConnectionPoint iOutPoint, Action<NodeConnection> OnClickRemoveConnection)
		{
			this.InPoint = iInPoint;
			this.OutPoint = iOutPoint;
			this.OnClickRemoveConnection = OnClickRemoveConnection;
		}

		public NodeConnection(NodeConnection aConnection) : this(aConnection.InPoint, aConnection.OutPoint, aConnection.OnClickRemoveConnection)
		{
			//Copy Constructor
		}

		public void Draw()
		{
			Handles.DrawBezier(
				InPoint.ConnectionPointRect.center,
				OutPoint.ConnectionPointRect.center,
				InPoint.ConnectionPointRect.center + Vector2.left * 50f,
				OutPoint.ConnectionPointRect.center - Vector2.left * 50f,
				Color.white,
				null,
				2f
				);

			isClicked = Handles.Button((InPoint.ConnectionPointRect.center + OutPoint.ConnectionPointRect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap);
		}

		public void ProcessEvents(Event iEvent)
		{
			if (isClicked)
			{
				if (OnClickRemoveConnection != null)
				{
					OnClickRemoveConnection(this);
				}
			}
		}
	}
}