using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System;
using NodeSystem;

public class DialogSystemEditor : EditorWindow
{
	private List<NodeBase> Nodes;
	private List<NodeConnection> Connections;

	private NodeConnectionPoint SelectedInPoint;
	private NodeConnectionPoint SelectedOutPoint;

	private Vector2 mOffset;
	private Vector2 mDrag;
	private ToolBar mToolbar;

	[MenuItem("God Nine Tools/Dialog System Editor")]
	private static void OpenWindow()
	{
		DialogSystemEditor window = GetWindow<DialogSystemEditor>();
		window.titleContent = new GUIContent("Dialog System Editor");
	}

	private void OnEnable()
	{
		mToolbar = new ToolBar(this);
	}

	private void OnGUI()
	{
		DrawCanvas(20, 0.2f, Color.gray);//小方格
		DrawCanvas(100, 0.4f, Color.gray);//大方格
		 DrawNodes();
		 DrawConnections();
		 DrawConnectionLine(Event.current);
		 DrawToolbar();

		 //////////Process//////////
		 ProcessToolbar(Event.current);
		 ProcessConnections(Event.current);
		 ProcessNodes(Event.current);
		 ProcessCanvas(Event.current);


		if (GUI.changed)
		{
			Repaint();
		}
	}
	//********************************************************************************
	// Process Functions
	//********************************************************************************
	#region Process Functions
	private void ProcessNodes(Event iEvent)
	{
		if (Nodes != null)
		{
			for (int i = Nodes.Count - 1; i >= 0; i--)
			{
				bool guiChanged = Nodes[i].ProcessEvents(iEvent);

				if (guiChanged)
				{
					GUI.changed = true;
				}
			}
		}
	}

	private void ProcessConnections(Event iEvent)
	{
		if (Connections != null)
		{
			for (int i = Connections.Count - 1; i >= 0; i--)
			{
				Connections[i].ProcessEvents(iEvent);
			}
		}
	}

	private void ProcessCanvas(Event iEvent)
	{
		mDrag = Vector2.zero;

		switch (iEvent.type)
		{
			case EventType.MouseDown:
				if (iEvent.button == 0)
				{
					ClearConnectionSelection();
				}
				if (iEvent.button == 1)
				{
					ProcessContextMenu(iEvent.mousePosition);
				}
				break;
			case EventType.MouseDrag:
				if (iEvent.button == 0)
				{
					OnDrag(iEvent.delta);
				}
				break;
		}
	}

//	private void ProcessToolbar(Event e)
//	{
//		toolbar.ProcessEvents(e);
//	}
	#endregion

	//********************************************************************************
	// Draw Functions
	//********************************************************************************
	#region Draw Functions
	private void DrawCanvas(float iGridSpacing, float iGridAlpha, Color gridColor)
	{
		int widthdivs = Mathf.CeilToInt(position.width / iGridSpacing);
		int heightDivs = Mathf.CeilToInt(position.height / iGridSpacing);

		Handles.BeginGUI();
		Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, iGridAlpha);

		mOffset += mDrag * 0.5f;
		Vector3 newOffset = new Vector3(mOffset.x % iGridSpacing, mOffset.y % iGridSpacing, 0);

		for (int i = 0; i < widthdivs; i++)
		{
			Handles.DrawLine(new Vector3(iGridSpacing * i, -iGridSpacing, 0) + newOffset, new Vector3(iGridSpacing * i, position.height, 0f) + newOffset);
		}

		for (int i = 0; i < heightDivs; i++)
		{
			Handles.DrawLine(new Vector3(-iGridSpacing, iGridSpacing * i, 0) + newOffset, new Vector3(position.width, iGridSpacing * i, 0f) + newOffset);
		}

		Handles.color = Color.white;
		Handles.EndGUI();
	}


	private void DrawNodes()
	{
		if (Nodes != null)
		{
			for (int i = 0; i < Nodes.Count; i++)
			{
				Nodes[i].Draw();
			}
		}
	}

	private void DrawConnections()
	{
		if (Connections != null)
		{
			for (int i = 0; i < Connections.Count; i++)
			{
				Connections[i].Draw();
			}
		}
	}

	private void DrawConnectionLine(Event iEvent)
	{
		if (SelectedInPoint != null && SelectedOutPoint == null)
		{
			Handles.DrawBezier(
				SelectedInPoint.ConnectionPointRect.center,
				iEvent.mousePosition,
				SelectedInPoint.ConnectionPointRect.center + Vector2.left * 50f,
				iEvent.mousePosition - Vector2.left * 50f,
				Color.magenta,
				null,
				2f
				);

			GUI.changed = true;
		}

		if (SelectedOutPoint != null && SelectedInPoint == null)
		{
			Handles.DrawBezier(
				SelectedOutPoint.ConnectionPointRect.center,
				iEvent.mousePosition,
				SelectedOutPoint.ConnectionPointRect.center - Vector2.left * 50f,
				iEvent.mousePosition + Vector2.left * 50f,
				Color.magenta,
				null,
				2f
				);

			GUI.changed = true;
		}
	}

	private void DrawToolbar()
	{
		mToolbar.Draw();
	}
	#endregion

	//********************************************************************************
	// Node Editor  Private Functions
	//********************************************************************************
	#region Node Editor  Private Functions
	private void ProcessToolbar(Event iEvent)
	{
		mToolbar.ProcessEvents(iEvent);
	}

	private void AddNode(NodeBase iNode)
	{
		if (Nodes == null)
		{
			Nodes = new List<NodeBase>();
		}

		//look for start node, reject if start node exists
		if (iNode.GetType() == typeof(InitNode))
		{
			foreach (NodeBase aNode in Nodes)
			{
				if (aNode.GetType() == typeof(InitNode))
				{
					//toolbar.setMessage("Cannot create more than 1 Start Node");
					return;
				}
			}
		}

		Nodes.Add(iNode);
	}

	private void ProcessContextMenu(Vector2 mousePosition)
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Init Node"), false, () => AddNode(new InitNode(this, mousePosition)));
		//genericMenu.AddItem(new GUIContent("End Node"), false, () => AddNode(new EndNode(this, mousePosition)));
		genericMenu.AddItem(new GUIContent("Action Node"), false, () => AddNode(new ActionNode(this, mousePosition)));
		genericMenu.ShowAsContext();
	}

	public void OnClickInPoint(NodeConnectionPoint iInPoint)
	{
		SelectedInPoint = iInPoint;


		if (SelectedOutPoint != null)
		{
			if (SelectedInPoint.Node != SelectedOutPoint.Node)
			{
				CreateConnection();
				ClearConnectionSelection();
			}
			else {
				ClearConnectionSelection();
			}
		}
	}

	public void OnClickOutPoint(NodeConnectionPoint iOutPoint)
	{
		SelectedOutPoint = iOutPoint;

		if (SelectedInPoint != null)
		{
			if (SelectedOutPoint.Node != SelectedInPoint.Node)
			{
				CreateConnection();
				ClearConnectionSelection();
			}
			else {
				ClearConnectionSelection();
			}
		}
	}

	private void CreateConnection()
	{
		if (Connections == null)
		{
			Connections = new List<NodeConnection>();
		}

		//prevents creating the same connection twice
		//bool connectionExists = Connections.Any(item => item.InPoint == SelectedInPoint && item.OutPoint == SelectedOutPoint);

		//allows only one connection for each outgoing connection point
		bool out_exists = false;
		for (int i = 0; i < Connections.Count; i++)
		{
			if (Connections[i].OutPoint == SelectedOutPoint)
			{
				out_exists = true;
			}
		}

		if (/*!connectionExists*/ !out_exists)
		{
			Connections.Add(new NodeConnection(SelectedInPoint, SelectedOutPoint, RemoveConnection));
		}
	}

	private void RemoveConnection(NodeConnection iConnection)
	{
		Connections.Remove(iConnection);
	}

	private void ClearConnectionSelection()
	{
		SelectedInPoint = null;
		SelectedOutPoint = null;
	}

	private void OnDrag(Vector2 delta)
	{
		mDrag = Vector2.zero + delta;

		if (Nodes != null)
		{
			for (int i = 0; i < Nodes.Count; i++)
			{
				Nodes[i].Drag(delta);
			}
		}

		GUI.changed = true;
	}
	#endregion

	//********************************************************************************
	// Node Editor  Public Functions
	//********************************************************************************
	#region Node Editor  Public Functions
	public void OnClickRemoveNode(NodeBase iNode)
	{
		if (Connections != null)
		{
			List<NodeConnection> aConnectionsToRemove = new List<NodeConnection>();

			for (int i = 0; i < Connections.Count; i++)
			{
				if (Connections[i].InPoint.Node == iNode || Connections[i].OutPoint.Node == iNode)
				{
					aConnectionsToRemove.Add(Connections[i]);
				}
			}

			for (int i = 0; i < aConnectionsToRemove.Count; i++)
			{
				Connections.Remove(aConnectionsToRemove[i]);
			}
		}

		Nodes.Remove(iNode);
	}
	#endregion

	//********************************************************************************
	// Tool Bar Functions
	//********************************************************************************
	#region Tool Bar Functions
	public string SaveCanvas(string iPath)
	{
		//TODO better way than try catch block AND change to return bool?
		try
		{
			if (Nodes == null) Nodes = new List<NodeBase>();
			if (Connections == null) Connections = new List<NodeConnection>();
			EditorSaveNodeData aSave = BuildSaveObject();
			AssetDatabase.CreateAsset(aSave, iPath);
		}
		catch (Exception e)
		{
			return e.Message;
		}
		return null;
	}

	public string LoadCanvas(string iPath)
	{
		//SAME AS SAVE CANVAS
		try
		{
			//EditorSaveObject load = Resources.Load(path) as EditorSaveObject;
			EditorSaveNodeData aLoad = AssetDatabase.LoadAssetAtPath(iPath, typeof(EditorSaveNodeData)) as EditorSaveNodeData;

			//build new CP / Index
			List<NodeConnectionPoint> CPIndex = new List<NodeConnectionPoint>();
			for (int i = 0; i < aLoad.NumberOfCP; i++)
			{
				CPIndex.Add(new NodeConnectionPoint());
			}

			//build Nodes
			int spent = 0; //tracks index of used CP
			Nodes = new List<NodeBase>();
			for (int i = 0; i < aLoad.NodeDatas.Count; i++)
			{
				Type t = Type.GetType(aLoad.NodeDatas[i].Type);
				ConstructorInfo ctor = t.GetConstructor(new[] { GetType(), typeof(NodeData) });
				NodeBase aNode = (NodeBase)Convert.ChangeType(ctor.Invoke(new object[] { this, aLoad.NodeDatas[i] }), t);
				aNode.Rebuild(CPIndex.GetRange(spent, aLoad.NodeCPIndex[i]));
				spent += aLoad.NodeCPIndex[i];
				AddNode(aNode);
			}

			//build Connections
			Connections = new List<NodeConnection>();
			for (int i = 0; i < aLoad.ConnectionIndexIn.Count; i++)
			{
				Connections.Add(new NodeConnection(CPIndex[aLoad.ConnectionIndexIn[i]], CPIndex[aLoad.ConnectionIndexOut[i]], RemoveConnection));
			}

			mOffset = new Vector2(aLoad.offset.x, aLoad.offset.y);
			mDrag = Vector2.zero;
		}
		catch (Exception e)
		{
			return e.Message;
		}
		return null;
	}

	private EditorSaveNodeData BuildSaveObject()
	{
		//Build CP array and Node CP index for reference
		List<NodeConnectionPoint> CPArray = new List<NodeConnectionPoint>();
		List<int> NodeCPIndex = new List<int>();
		for (int i = 0; i < Nodes.Count; i++)
		{
			List<NodeConnectionPoint> t = Nodes[i].GetConnectionPoints();
			CPArray.AddRange(t);
			NodeCPIndex.Add(t.Count);
		}

		//Build Connection Reference Index
		List<int> ConnectionIndexIn = new List<int>();
		List<int> ConnectionIndexOut = new List<int>();
		for (int i = 0; i < Connections.Count; i++)
		{
			ConnectionIndexIn.Add(CPArray.IndexOf(Connections[i].InPoint));
			ConnectionIndexOut.Add(CPArray.IndexOf(Connections[i].OutPoint));
		}

		//Build Node Info
		List<NodeData> aNodeDatas = new List<NodeData>();
		for (int i = 0; i < Nodes.Count; i++)
		{
			aNodeDatas.Add(Nodes[i].GetInfo());
		}

		//Return Save Object
		EditorSaveNodeData save = CreateInstance<EditorSaveNodeData>();
		save.init(aNodeDatas, NodeCPIndex, ConnectionIndexIn, ConnectionIndexOut, CPArray.Count, mOffset);
		return save;
	}

	public void BuildCanvas(string path)
	{
		//creates friendly version for loading at runtime
		if (Nodes == null) Nodes = new List<NodeBase>();
		if (Connections == null) Connections = new List<NodeConnection>();
		List<BuildNode> aBuildNodes = new List<BuildNode>();

		//build node array
		List<NodeBase> node_index_reference = new List<NodeBase>();
		for (int i = 0; i < Nodes.Count; i++)
		{
			if (Nodes[i].GetType() == typeof(ActionNode))
			{
				node_index_reference.Add(Nodes[i]);
				NodeData aNodeData = Nodes[i].GetInfo();
				aBuildNodes.Add(new BuildNode(aNodeData.NodeName, aNodeData.Triggers));
			}
		}

		//build next indexes //indices?
		for (int i = 0; i < node_index_reference.Count; i++)
		{
			for (int j = 0; j < Connections.Count; j++)
			{
				if (Connections[j].OutPoint.Node == node_index_reference[i])
				{
					int index_of_next = node_index_reference.IndexOf(Connections[j].InPoint.Node);
					int index_of_trigger = ((ActionNode)node_index_reference[i]).OutPoints.IndexOf(Connections[j].OutPoint);
					aBuildNodes[i].NextIndex[index_of_trigger] = index_of_next;
				}
			}
		}

		//get starting DialogNode
		NodeBase start_n = null;
		for (int i = 0; i < Nodes.Count; i++)
		{
			if (Nodes[i].GetType() == typeof(InitNode))
			{
				start_n = Nodes[i];
			}
		}
		if (start_n == null)
		{
			Debug.LogError("No Start Node");
			return;
		}
		int starting_index = -1;
		for (int i = 0; i < Connections.Count; i++)
		{
			if (Connections[i].OutPoint.Node == start_n)
			{
				starting_index = node_index_reference.IndexOf(Connections[i].InPoint.Node);
			}
		}
		if (starting_index < 0)
		{
			Debug.LogError("Start Node not connected to anything");
			return;
		}

		BuildNodeObject build = CreateInstance<BuildNodeObject>();
		build.Init(aBuildNodes, starting_index, starting_index);

		//
		AssetDatabase.CreateAsset(build, path);
	}

	public void MoveToStart()
	{
		foreach (NodeBase aNode in Nodes)
		{
			if (aNode.GetType() == typeof(InitNode))
			{
				float x = aNode.NodeRect.x + (aNode.NodeRect.width / 2);
				float y = aNode.NodeRect.y + (aNode.NodeRect.height / 2);
				float x_base = position.width / 2;
				float y_base = position.height / 2;
				OnDrag(new Vector2(x_base - x, y_base - y));
			}
		}
	}
	#endregion
}