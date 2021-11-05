using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeSimple : GamemodeBase
{
	
	public Transform[] SpawnPoints;
	
	public override Transform GetSpawnPoint(PawnControllerBase c)
	{
		return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
	}

	public override bool CanAlwaysSpawnPawns()
	{
		return true;
	}
}