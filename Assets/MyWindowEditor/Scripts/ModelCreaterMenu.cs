using UnityEngine;
using System.Collections;
using UnityEditor;
public class ModelCreaterMenu : EditorWindow
{
	[MenuItem("ModelCreaterMenu/Main Control Panel", false, 0)]
	static void ModelCreater()
	{
		EditorWindow.GetWindow(typeof(ModelCreater));
	}
}