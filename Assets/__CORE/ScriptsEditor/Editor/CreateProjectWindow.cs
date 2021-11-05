using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using Tymski;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class CreateProjectWindow : EditorWindow
{
	private string _projectName = "New Project";
	private string _projectPrefix = "NP";

	[MenuItem("Beeworks/Create Project")]
	private static void Init()
	{
		CreateProjectWindow w = (CreateProjectWindow) GetWindow(typeof(CreateProjectWindow));
		w.Show();
	}

	private void OnGUI()
	{
		EditorGUILayout.LabelField("Project name:");
		_projectName = EditorGUILayout.TextField(_projectName);
		_projectPrefix = EditorGUILayout.TextField(_projectPrefix);

		if (GUILayout.Button("Create project!"))
		{
			CreateProject();
		}
	}

	private void CreateProject()
	{
		string cf = AssetDatabase.CreateFolder("Assets/__PROJECTS", _projectName);
		if (cf.Length <= 0)
		{
			Debug.LogError("Error: Could not create a new folder for the project! Did you delete the __PROJECTS folder?");
			return;
		}

		AssetDatabase.CreateFolder("Assets/__PROJECTS/" + _projectName, "Scenes");
		AssetDatabase.CreateFolder("Assets/__PROJECTS/" + _projectName, "Scripts");
		AssetDatabase.CreateFolder("Assets/__PROJECTS/" + _projectName, "Prefabs");

		string tempGameScenePath = Application.dataPath + "/__CORE/Scenes/TEMPLATE_GameScene.unity";
		string tempGameSceneDestinationPath = Application.dataPath + "/__PROJECTS/" + _projectName + "/Scenes/" + _projectPrefix + "_02_Game.unity";

		string tempMenuScenePath = Application.dataPath + "/__CORE/Scenes/TEMPLATE_MenuScene.unity";
		string tempMenuSceneDestinationPath = Application.dataPath + "/__PROJECTS/" + _projectName + "/Scenes/" + _projectPrefix + "_01_Menu.unity";

		File.Copy(tempGameScenePath, tempGameSceneDestinationPath);
		File.Copy(tempMenuScenePath, tempMenuSceneDestinationPath);

		SO_GameDef gd = CreateInstance<SO_GameDef>();
		gd.GameName = _projectName;
		gd.OfflineScene = new SceneReference();
		gd.OnlineScene = new SceneReference();

		string gdPath = "Assets/__PROJECTS/" + _projectName + "/SO_GD_" + _projectName + ".asset";

		AssetDatabase.CreateAsset(gd, gdPath);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		gd = AssetDatabase.LoadAssetAtPath<SO_GameDef>(gdPath);

		gd.OfflineScene.ScenePath = "Assets/__PROJECTS/" + _projectName + "/Scenes/" + _projectPrefix + "_01_Menu.unity";
		gd.OnlineScene.ScenePath = "Assets/__PROJECTS/" + _projectName + "/Scenes/" + _projectPrefix + "_02_Game.unity";

		GameObject o = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/__CORE/Prefabs/Pawn - Sample.prefab", typeof(GameObject));
		GameObject p = PrefabUtility.InstantiatePrefab(o) as GameObject;

		GameObject pawnPrefab = PrefabUtility.SaveAsPrefabAsset(p, "Assets/__PROJECTS/" + _projectName + "/Prefabs/Pawn - " + _projectPrefix + ".prefab");


		gd.NetworkedPrefabs = new[] {pawnPrefab.GetComponent<NetworkIdentity>()};

		//AssemblyDefinitionAsset ada = 

		EditorUtility.SetDirty(gd);
		AssetDatabase.SaveAssets();

		Selection.activeObject = gd;
		EditorGUIUtility.PingObject(gd);

		DestroyImmediate(p);
	}
}