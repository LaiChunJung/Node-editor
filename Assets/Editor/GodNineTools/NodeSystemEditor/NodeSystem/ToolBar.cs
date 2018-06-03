﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NodeSystem
{
	public class ToolBar
	{
		private DialogSystemEditor Editor;
		private Rect rect;
		private bool clickedSave;
		private bool clickedSaveAs;
		private bool clickedLoad;
		private bool clickedBuild;
		private bool clickedMoveToStart;
		private string path;
		private string message;

		public ToolBar(DialogSystemEditor Editor)
		{
			this.Editor = Editor;
			rect = new Rect(0, 0, this.Editor.position.width, 500); //find flexible text width!!!
		}

		public void Draw()
		{
			rect.width = Editor.position.width;

			GUILayout.BeginArea(rect, EditorStyles.toolbar);

			GUILayout.BeginHorizontal();

			clickedSave = GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(50));
			clickedSaveAs = GUILayout.Button(new GUIContent("Save As"), EditorStyles.toolbarButton, GUILayout.Width(50));
			clickedLoad = GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(50));
			clickedBuild = GUILayout.Button(new GUIContent("Build"), EditorStyles.toolbarButton, GUILayout.Width(50));
			clickedMoveToStart = GUILayout.Button(new GUIContent("Move to Start"), EditorStyles.toolbarButton, GUILayout.Width(100));

			if (path != null)
			{
				float width = (new GUIStyle()).CalcSize((new GUIContent(path))).x;
				GUILayout.Label(path, GUILayout.Width(width + 20f));
			}

			GUILayout.FlexibleSpace();

			if (message != null)
			{
				float width = (new GUIStyle()).CalcSize((new GUIContent(message))).x;
				GUILayout.Label(message, GUILayout.Width(width + 20f));
			}

			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		public void ProcessEvents(Event e)
		{
			if (clickedSave)
			{
				if (path != null)
				{
					string status = Editor.SaveCanvas(path);
					if (status != null)
					{
						message = status;
					}
					else {
						message = "Save Success at: " + System.DateTime.Now;
					}
				}
				else {
					ProcessSaveAs();
				}
			}

			if (clickedSaveAs)
			{
				ProcessSaveAs();
			}

			if (clickedLoad)
			{
				string local_path = EditorUtility.OpenFilePanel("Load Canvas", "Assets", "asset");
				if (local_path.Length > "Assets".Length)
				{
					local_path = local_path.Substring(Application.dataPath.Length - "Assets".Length);
					string status = Editor.LoadCanvas(local_path);
					if (status != null)
					{
						message = status;
					}
					else {
						path = local_path;
						message = "Load Success at: " + System.DateTime.Now;
					}
				}
			}

			if (clickedBuild)
			{
				string path = EditorUtility.SaveFilePanelInProject("Save Build", "Build.asset", "asset", "");
				if (path.Length > 0)
				{
					Editor.BuildCanvas(path);
				}
			}

			if (clickedMoveToStart)
			{
				Editor.MoveToStart();
			}
		}

		private void ProcessSaveAs()
		{
			string local_path = EditorUtility.SaveFilePanelInProject("Save Canvas", "Canvas.asset", "asset", "");
			if (local_path.Length > 0)
			{
				string status = Editor.SaveCanvas(local_path);
				if (status != null)
				{
					message = status;
				}
				else {
					path = local_path;
					message = "Save Success at: " + System.DateTime.Now;
				}
			}
		}

		public void setMessage(string msg)
		{
			message = msg;
		}
	}
}