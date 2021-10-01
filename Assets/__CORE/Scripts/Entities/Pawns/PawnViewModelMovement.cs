using System.Collections;
using System.Collections.Generic;
using CMF;
using UnityEngine;

public class PawnViewModelMovement : PawnComponent
{
	[Header("Objects")]
	public Transform BobTransform;
	public Transform TiltTransform;
	[Header("Settings - Bobbing")]
	public float HeightAmplitude = .005f;
	public float SideAmplitude = .005f;
	public float Frequency = 1f;
	public float BobLerpRate = 15f;
	[Header("Settings - Tilting")]
	public float TiltIntensity = 1f;
	public float TiltLerpRate = 15f;


	private PawnMotor _motor;
	private Vector3 _currentLocalVelocity;
	private float _xVel;
	private float _zVel;


	protected override void Initialize()
	{
		base.Initialize();
		_motor = GetComponent<PawnMotor>();
	}

	public override void LateTick()
	{
		base.LateTick();

		if (!IsOwner()) return;

		_currentLocalVelocity = _motor.GetLocalVelocity();
		float maxSpeed = _motor.Controller.movementSpeed;

		_zVel = _currentLocalVelocity.z;
		_zVel = Mathf.Clamp(_zVel, -maxSpeed, maxSpeed) / maxSpeed;

		_xVel = _currentLocalVelocity.x;
		_xVel = Mathf.Clamp(_xVel, -maxSpeed, maxSpeed) / maxSpeed;

		Bob();
		Tilt();
	}

	private void Bob()
	{
		float frTime = Time.timeSinceLevelLoad * Frequency;
		float sin = Mathf.Sin(frTime);

		float yPos = sin * HeightAmplitude * _zVel;
		float xPos = sin * SideAmplitude * _xVel;
		BobTransform.localPosition = Vector3.Lerp(BobTransform.localPosition, new Vector3(xPos, yPos, 0f), Time.deltaTime * BobLerpRate);
	}

	private void Tilt()
	{
		Vector3 leuler = TiltTransform.localEulerAngles;
		leuler.z = Mathf.LerpAngle(leuler.z, -Mathf.Abs(TiltIntensity) * _xVel, Time.deltaTime * TiltLerpRate);
		TiltTransform.localEulerAngles = leuler;
	}
}