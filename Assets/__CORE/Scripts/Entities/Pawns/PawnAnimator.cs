using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAnimator : PawnComponent
{
	[Header("Objects")]
	public Animator Anim;
	[Header("Settings")]
	public float MovementLerpRate = 20f;

	private PawnMotor _motor;
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

		if (!IsOwner()) return;

		Vector3 vel = _motor.GetLocalVelocity();
		float xVel = vel.x / _motor.Controller.movementSpeed;
		float zVel = vel.z / _motor.Controller.movementSpeed;

		float lerpRate = Time.deltaTime * MovementLerpRate;

		_lastVelX = Mathf.Lerp(_lastVelX, xVel, lerpRate);
		_lastVelZ = Mathf.Lerp(_lastVelZ, zVel, lerpRate);
		
		

		Anim.SetFloat(SPEED_X, _lastVelX);
		Anim.SetFloat(SPEED_Z, _lastVelZ);

		Anim.SetBool(GROUNDED, _motor.Controller.IsGrounded());
	}
}