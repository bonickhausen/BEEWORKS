using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class PawnIKHandler : PawnComponent
{

	[Header("Objects")]
	public FullBodyBipedIK FBBIK;
	public Transform LeftHandTransform;
	public Transform RightHandTransform;

	private PawnWeaponHandler _weaponHandler;

	protected override void Initialize()
	{
		_weaponHandler = GetComponent<PawnWeaponHandler>();
	}

	public override void Tick()
	{
		ItemWeapon weapon = _weaponHandler.CurrentWeapon;
		if (weapon != null)
		{
			FBBIK.solver.leftHandEffector.positionWeight = 1f;
			FBBIK.solver.leftHandEffector.rotationWeight = 1f;
			FBBIK.solver.rightHandEffector.positionWeight = 1f;
			FBBIK.solver.rightHandEffector.rotationWeight = 1f;
			FBBIK.solver.leftHandEffector.target = weapon.LeftHandTransform;
			FBBIK.solver.rightHandEffector.target = weapon.RightHandTransform;
		}
		else
		{
			FBBIK.solver.leftHandEffector.positionWeight = 0f;
			FBBIK.solver.leftHandEffector.rotationWeight = 0f;
			FBBIK.solver.rightHandEffector.positionWeight = 0f;
			FBBIK.solver.rightHandEffector.rotationWeight = 0f;
		}
	}
}