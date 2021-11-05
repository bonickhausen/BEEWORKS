using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using Object = UnityEngine.Object;

public class AssetHandler
{
	[OnOpenAsset]
	public static bool OpenAsset(int instanceID)
	{
		Object obj = null;
		try
		{
			obj = EditorUtility.InstanceIDToObject(instanceID);
		}
		catch (Exception)
		{
			// ignored
		}
		SO_GameDef gd = obj as SO_GameDef;
		if (gd == null) return false;
		GameHandler gh = Object.FindObjectOfType<GameHandler>();
		if (gh == null) return false;
		gh.GameDefinition = gd;
		gh.LoadGameDefinition();
		return true;
	}
}