using System;
using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using Mirror;
using Tymski;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
	[Header("Game Definition Settings")]
	public SO_GameDef GameDefinition;
	[Header("Global Settings")]
	public NetworkIdentity[] GlobalPrefabs;
	public PawnControllerBase DefaultController;

	private NetworkManager _netMan;

	public static GameHandler Instance { get; private set; }
	public static bool HasInitialized = false;

	private void Awake()
	{
		if (!GameDefinition) return;

		Instance = this;
		_netMan = GetComponent<NetworkManager>();
	}

	private void Start()
	{
		HasInitialized = true;

		if (GameDefinition.ShouldHostOnStart)
		{
			GameDefinition.ShouldHostOnStart = false;
			Host();
		}
	}

	private void OnValidate()
	{
		LoadGameDefinition();
	}

	public void LoadGameDefinition()
	{
#if UNITY_EDITOR
		if (GameDefinition)
		{
			// Load scenes into build settings
			EditorBuildSettingsScene s0 = new EditorBuildSettingsScene(SceneManager.GetActiveScene().path, true);
			EditorBuildSettingsScene s1 = new EditorBuildSettingsScene(GameDefinition.OfflineScene.ScenePath, true);
			EditorBuildSettingsScene s2 = new EditorBuildSettingsScene(GameDefinition.OnlineScene.ScenePath, true);
			EditorBuildSettings.scenes = new[] {s0, s1, s2};

			// Get ref to netman
			_netMan = GetComponent<NetworkManager>();

			if (GameDefinition.ControllerOverride)
			{
				_netMan.playerPrefab = GameDefinition.ControllerOverride.gameObject;
			}
			else
			{
				_netMan.playerPrefab = DefaultController.gameObject;
			}

			// Load em into the network manager
			_netMan.offlineScene = GameDefinition.OfflineScene;
			_netMan.onlineScene = GameDefinition.OnlineScene;

			// Load prefabs
			_netMan.spawnPrefabs = new List<GameObject>();
			foreach (NetworkIdentity p in GlobalPrefabs)
			{
				if (p != null)
				{
					_netMan.spawnPrefabs.Add(p.gameObject);
				}
			}
			foreach (NetworkIdentity p in GameDefinition.NetworkedPrefabs)
			{
				if (p != null)
				{
					_netMan.spawnPrefabs.Add(p.gameObject);
				}
			}

			Debug.Log("A game definition file has been loaded: " + GameDefinition.GameName);
		}
		else
		{
			Debug.Log("No game definition file found.");
		}
#endif
	}

	public void Host()
	{
		_netMan.StartHost();
	}

	public void Join(string ip)
	{
		_netMan.networkAddress = ip;
		_netMan.StartClient();
	}

	[ConsoleMethod("host", "Host a server."), UnityEngine.Scripting.Preserve]
	public static void SvHost()
	{
		Instance.Host();
	}

	[ConsoleMethod("join", "Join a server."), UnityEngine.Scripting.Preserve]
	public static void ClJoin(string ip)
	{
		Instance.Join(ip);
	}
}