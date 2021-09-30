using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Tymski;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Game Definition", menuName = "BEEWORKS/SO/Game Definition")]
public class SO_GameDef : ScriptableObject
{
	[Header("Essentials")]
	public string GameName;
	public SceneReference OfflineScene;
	public SceneReference OnlineScene;
	public NetworkIdentity[] Prefabs;
	[Header("Overrides")]
	public PawnControllerBase ControllerOverride;

	[HideInInspector]
	public bool ShouldHostOnStart;

#if UNITY_EDITOR
	private void OnValidate()
	{
		GameHandler gh = FindObjectOfType<GameHandler>();
		if (gh)
		{
			gh.LoadGameDefinition();
		}
	}
#endif
}