using System;
using System.Collections;
using System.Collections.Generic;
using CMF;
using UnityEngine;

public class PawnMotor : PawnComponent
{
	public AdvancedWalkerController Controller;

	public CORE_Delegates.VoidDelegate Evnt_OnGroundedChanged;

	public Vector3 Direction { get; private set; }

	private PawnMotorInputRelay _inputRelay;
	private Rigidbody _rb;
	private bool _wasGroundedLastFrame;

	private void Awake()
	{
		_inputRelay = Controller.GetComponent<PawnMotorInputRelay>();
		_rb = Controller.GetComponent<Rigidbody>();
	}

	public Transform GetTransform()
	{
		return Controller.transform;
	}

	public bool IsGrounded()
	{
		return Controller.IsGrounded();
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
		if (IsOwner())
		{
			_inputRelay.SetCommand(_cmd);
			bool isGrounded = IsGrounded();

			if (isGrounded != _wasGroundedLastFrame)
			{
				OnGroundedChanged();
			}

			_wasGroundedLastFrame = isGrounded;

			Vector3 cVel = GetWorldVelocity();
			
			if (cVel.magnitude > 0)
			{
				Direction = cVel.normalized;
			}
		}

		void OnGroundedChanged()
		{
			Evnt_OnGroundedChanged?.Invoke();
		}
	}
}