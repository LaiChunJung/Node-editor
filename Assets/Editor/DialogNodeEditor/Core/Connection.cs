using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace NodeSystem
{
    public class Connection
	{
        public ConnectionPoint InPoint;
        public ConnectionPoint OutPoint;

        public Action<Connection> OnClickRemoveConnection;

        public bool isClicked = false;

        public Connection(ConnectionPoint iInPoint, ConnectionPoint iOutPoint, Action<Connection> iOnClickRemoveConnection) {
            InPoint = iInPoint;
            OutPoint = iOutPoint;
            OnClickRemoveConnection = iOnClickRemoveConnection;
        }

		public Connection(Connection iConnection) : this(iConnection.InPoint, iConnection.OutPoint, iConnection.OnClickRemoveConnection)	{}

		public void Draw()
		{
			Handles.DrawBezier(
				InPoint.rect.center,
				OutPoint.rect.center,
				InPoint.rect.center + Vector2.left * 50f,
				OutPoint.rect.center - Vector2.left * 50f,
				Color.white,
				null,
				2f
				);

			isClicked = Handles.Button((InPoint.rect.center + OutPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap);
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