using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorQuickPlay : MonoBehaviour
{
	[MenuItem("Beeworks/Play Current Game &b")]
	public static void QuickPlay()
	{
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
		GameHandler gh = FindObjectOfType<GameHandler>();
		gh.GameDefinition.ShouldHostOnStart = true;
		EditorApplication.isPlaying = true;
	}
}