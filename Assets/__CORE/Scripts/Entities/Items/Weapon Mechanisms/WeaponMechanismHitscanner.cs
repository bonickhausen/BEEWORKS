using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMechanismHitscanner : WeaponMechanismBase
{
	public MuzzleFlasher MuzzleFlash;

	protected override void UseWithNoAmmo()
	{ }

	protected override void UseMechanism()
	{ }

	protected override void ProduceUsageFX()
	{
		MuzzleFlash.Flash();
	}
}