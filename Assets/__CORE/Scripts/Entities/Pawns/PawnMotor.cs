using System;
using System.Collections;
using System.Collections.Generic;
using CMF;
using UnityEngine;

public class PawnMotor : PawnComponent
{
	public AdvancedWalkerController Controller;

	private PawnMotorInputRelay _inputRelay;
	private Rigidbody _rb;

	private void Awake()
	{
		_inputRelay = Controller.GetComponent<PawnMotorInputRelay>();
		_rb = Controller.GetComponent<Rigidbody>();
	}

	public Transform GetTransform()
	{
		return Controller.transform;
	}

	public Rigidbody GetRigidbody()
	{
		return _rb;
	}

	public Vector3 GetWorldVelocity()
	{
		return GetRigidbody().velocity;
	}

	public Vector3 GetLocalVelocity()
	{
		Vector3 wVel = GetWorldVelocity();
		Transform cTr = Controller.cameraTransform;
		return cTr.InverseTransformDirection(wVel);
	}

	public override void Tick()
	{
		_inputRelay.SetCommand(_cmd);
		_inputRelay.gameObject.SetActive(IsOwner());
	}
}