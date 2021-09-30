using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PawnViewBase : PawnComponent
{
	[Header("Setup")]
	public Transform ViewRig;

	protected PawnMotor _motor;

	public abstract bool ShouldShowSelfRenderer();

	protected override void Initialize()
	{
		base.Initialize();
		_motor = GetComponent<PawnMotor>();
	}
}