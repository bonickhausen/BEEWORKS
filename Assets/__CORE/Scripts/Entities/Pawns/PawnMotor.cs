using System;
using System.Collections;
using System.Collections.Generic;
using CMF;
using UnityEngine;

public class PawnMotor : PawnComponent
{
	public AdvancedWalkerController Controller;

	private PawnMotorInputRelay _inputRelay;

	private void Awake()
	{
		_inputRelay = Controller.GetComponent<PawnMotorInputRelay>();
	}

	public Transform GetTransform()
	{
		return Controller.transform;
	}

	public override void Tick()
	{
		_inputRelay.SetCommand(_cmd);
		_inputRelay.gameObject.SetActive(IsOwner());
	}
}