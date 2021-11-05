using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnControllerPlayer : PawnControllerBase
{
	public GameObject OwnerOnlyObject;

	public static PawnControllerPlayer LocalInstance;

	protected override void CacheComponents()
	{
		
	}

	protected override void Initialize()
	{
		OwnerOnlyObject.SetActive(false);
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		OwnerOnlyObject.SetActive(true);
		LocalInstance = this;
		if (GamemodeBase.Instance.CanAlwaysSpawnPawns())
		{
			RequestPawn();	
		}
	}

	private void RequestPawn()
	{
		PawnFactory.Instance.RequestPawn(this, 0);
	}
}