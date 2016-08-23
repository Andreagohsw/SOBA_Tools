using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

// SOBA Toolbox
// Various utilities that could be useful!

// Script by Jean Moreno - july 2016


public class SOBAToolbox : EditorWindow
{
	static GenericMenu menuToShow;

	[MenuItem("Window/SOBA Toolbox")]
	static void OpenWindow()
	{
		SOBAToolbox.GetWindow<SOBAToolbox>();
	}

	//--------------------------------------------------------------------------------------------------------------------------------

	void OnGUI()
	{
		GUILayout.Label("SOBA Toolbox", EditorStyles.boldLabel);
		DrawLine();
		GUILayout.Space(4f);

		if (GUILayout.Button("Select objects from layer..."))
		{
			var menu = new GenericMenu();
			string[] layers = GetLayerNames();
			for (int i = 0; i < layers.Length; i++)
			{
				menu.AddItem(new GUIContent(layers[i]), false, OnLayerSelected, i);
			}
			menu.ShowAsContext();
		}
	}

	//Small method to draw lines in GUI
	static public void DrawLine( float height = 1f )
	{
		DrawLine(Color.gray);
	}
	static public void DrawLine( Color color, float height = 1f )
	{
		var rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.label, GUILayout.Height(1f));
		var col = GUI.color;
		GUI.color = color;
		GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
		GUI.color = col;
	}

	//--------------------------------------------------------------------------------------------------------------------------------
	// Callbacks

	void OnLayerSelected( object layer )
	{
		int layerNb = (int)layer;
		var allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
		var objList = new List<GameObject>();
		foreach (var gameObj in allGameObjects)
		{
			if(gameObj.layer == layerNb)
				objList.Add(gameObj);
		}

		if (objList.Count == 0)
		{
			string layerName = LayerToName(layerNb);
			Debug.LogWarning(string.Format("No GameObject found in layer {0} in the current scene", layerName));
			EditorApplication.Beep();
		}
		else
			Selection.objects = objList.ToArray();
	}

	//--------------------------------------------------------------------------------------------------------------------------------
	// Utils

	string[] GetLayerNames()
	{
		var layers = new string[32];
		for (int i = 0; i < layers.Length; i++)
		{
			layers[i] = LayerToName(i);
			if (string.IsNullOrEmpty(layers[i]))
				layers[i] = string.Format("Layer {0}", i);
		}
		return layers;
	}

	string LayerToName(int layer)
	{
		string name = LayerMask.LayerToName(layer);
		if (string.IsNullOrEmpty(name))
			name = string.Format("Layer {0}", layer);
		return name;
	}
}
