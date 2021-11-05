using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamemodeBase : NetworkThing
{
	public static GamemodeBase Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public abstract Transform GetSpawnPoint(PawnControllerBase c);

	public abstract bool CanAlwaysSpawnPawns();
}