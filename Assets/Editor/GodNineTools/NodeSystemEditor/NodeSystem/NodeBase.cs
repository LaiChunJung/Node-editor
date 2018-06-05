//********************************************************************************
// Name spaces
//********************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace NodeSystem
{

	//********************************************************************************
	// Class
	//********************************************************************************
	public abstract class NodeBase
	{
		//********************************************************************************
		// public members
		//********************************************************************************
		public Rect NodeRect;
		public string NodeName;
		public DialogSystemEditor Editor;
		public GUIStyle Style = new GUIStyle();
		public float Width;
		public float Height;
		public bool IsDragged;
		//********************************************************************************
		// public methods
		//********************************************************************************
		public NodeBase(DialogSystemEditor iEditor, Vector2 position)
		{
			SetStyle();
			this.Editor = iEditor;
		}

		public NodeBase(DialogSystemEditor iEditor, NodeData iData)
		{
			SetStyle();
			this.Editor = iEditor;
		}

		//********************************************************************************
		// public virtual methods
		//********************************************************************************
		public virtual void Drag(Vector2 delta)
		{
			NodeRect.position += delta;
		}

		public virtual bool ProcessDefault(Event iEvent)
		{
			//adds clickdrag
			switch (iEvent.type)
			{
				case EventType.MouseDown:
					if (iEvent.button == 0)
					{
						if (NodeRect.Contains(iEvent.mousePosition))
						{
							IsDragged = true;
						}
					}
					else if (iEvent.button == 1 && NodeRect.Contains(iEvent.mousePosition))
					{
						//delete node
						GenericMenu genericMenu = new GenericMenu();
						genericMenu.AddItem(new GUIContent("Remove"), false, () => Editor.OnClickRemoveNode(this));
						genericMenu.ShowAsContext();
						iEvent.Use();
					}
					break;
				case EventType.MouseUp:
					IsDragged = false;
					break;
				case EventType.MouseDrag:
					if (iEvent.button == 0 && IsDragged)
					{
						Drag(iEvent.delta);
						iEvent.Use();
						return true;
					}
					break;
			}

			return false;
		}

		//********************************************************************************
		// public abstract methods
		//********************************************************************************
		public abstract void Init(Vector2 position);

		public abstract void Draw();

		public abstract bool ProcessEvents(Event e);

		public abstract void SetStyle();

		public abstract List<NodeConnectionPoint> GetConnectionPoints();

		public abstract NodeData GetInfo();

		public abstract void Rebuild(List<NodeConnectionPoint> iConnectionPoint);
	}

	[Serializable]
	public class NodeData
	{
		public string Type;
		public Rect NodeRect;
		public string NodeName;

		public List<string> Triggers;
		public List<Vector3> TargetPosition;

		public NodeData(string iType, Rect iRrect, string iName = null, List<string> iTriggers = null, List<Vector3> iTargetPosition = null)
		{
			this.Type = iType;
			this.NodeRect = iRrect;
			this.NodeName = iName;
			this.Triggers = iTriggers;
			this.TargetPosition = iTargetPosition;
		}
	}
}