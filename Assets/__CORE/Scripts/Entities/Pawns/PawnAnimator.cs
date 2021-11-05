using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PawnAnimator : PawnComponent
{
	[Header("Objects")]
	public Animator Anim;
	[Header("Settings")]
	public float MovementLerpRate = 20f;
	[Header("State")]
	[SyncVar]
	public float NetworkedSpeedX;
	[SyncVar]
	public float NetworkedSpeedZ;
	[SyncVar]
	public bool NetworkedGrounded;


	private PawnMotor _motor;
	private float _localVelX;
	private float _localVelZ;
	private bool _localGrounded;

	private float _lastVelX;
	private float _lastVelZ;

	private static readonly int SPEED_X = Animator.StringToHash("Speed X");
	private static readonly int SPEED_Z = Animator.StringToHash("Speed Z");
	private static readonly int GROUNDED = Animator.StringToHash("Grounded");

	private void Awake()
	{
		_motor = GetComponent<PawnMotor>();
	}

	public override void Tick()
	{
		base.Tick();

		float lerpRate = Time.deltaTime * MovementLerpRate;

		if (IsOwner())
		{
			GatherLocalParams();
			SendLocalParams();
		}
		else
		{
			ReadNetParams();
		}

		void GatherLocalParams()
		{
			Vector3 vel = _motor.GetLocalVelocity();
			_localVelX = vel.x / _motor.Controller.movementSpeed;
			_localVelZ = vel.z / _motor.Controller.movementSpeed;
			_localGrounded = _motor.Controller.IsGrounded();
		}

		void SendLocalParams()
		{
			if (!CanSendRPC()) return;

			CmdSendParams(new Vector2(_localVelX, _localVelZ), _localGrounded);
		}

		void ReadNetParams()
		{
			_localVelX = NetworkedSpeedX;
			_localVelZ = NetworkedSpeedZ;
			_localGrounded = NetworkedGrounded;
		}

		_lastVelX = Mathf.Lerp(_lastVelX, _localVelX, lerpRate);
		_lastVelZ = Mathf.Lerp(_lastVelZ, _localVelZ, lerpRate);


		Anim.SetFloat(SPEED_X, _lastVelX);
		Anim.SetFloat(SPEED_Z, _lastVelZ);

		Anim.SetBool(GROUNDED, _localGrounded);
	}

	[Command]
	private void CmdSendParams(Vector2 movement, bool isGrounded)
	{
		NetworkedSpeedX = movement.x;
		NetworkedSpeedZ = movement.y;
		NetworkedGrounded = isGrounded;
	}
}