using System.Collections;
using System.Collections.Generic;
using CMF;
using DG.Tweening;
using UnityEngine;

public class PawnViewModelMovement : PawnComponent
{
	[Header("Objects")]
	public Transform BobTransform;
	public Transform SwayTransform;
	public Transform TiltTransform;
	public Transform NudgeTransform;
	[Header("Settings - Bobbing")]
	public float HeightAmplitude = .005f;
	public float SideAmplitude = .005f;
	public float Frequency = 1f;
	public AnimationCurve BobCurve;
	public float BobLerpRate = 15f;
	[Header("Settings - Tilting")]
	public float TiltIntensity = 1f;
	public float TiltLerpRate = 15f;
	[Header("Settings - Nudges")]
	public float NudgeGroundedIntensity = 1f;
	public float NudgeGroundedDuration = 0.2f;
	public int NudgeGroundedVibrato = 5;
	public float NudgeJumpIntensity = 0.75f;
	public float NudgeJumpDuration = 0.6f;
	public int NudgeJumpVibrato = 1;
	[Header("Settings - Sway")]
	public float SwaybackRate = 10f;
	public float SwayVertSens = 1f;
	public float SwayHorzSens = 1f;


	private PawnMotor _motor;
	private Tweener _nudgeGroundedTweener;
	private Vector3 _currentLocalVelocity;
	private float _xVel;
	private float _zVel;


	protected override void Initialize()
	{
		base.Initialize();
		_motor = GetComponent<PawnMotor>();
		_motor.Evnt_OnGroundedChanged += OnGroundedChanged;
	}

	private void OnGroundedChanged()
	{
		bool isGrounded = _motor.IsGrounded();
		if (isGrounded)
		{
			Nudge(Vector3.right * NudgeGroundedIntensity, NudgeGroundedDuration, NudgeGroundedVibrato);
		}
		else
		{
			Nudge(Vector3.right * -NudgeJumpIntensity, NudgeJumpDuration, NudgeJumpVibrato);
		}
	}

	public void Nudge(Vector3 offset, float duration, int vibrato = 10)
	{
		if (_nudgeGroundedTweener != null)
		{
			DOTween.Kill(_nudgeGroundedTweener, true);
		}
		_nudgeGroundedTweener = NudgeTransform.DOPunchRotation(offset, duration, vibrato);
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
		Sway();
	}

	private void Bob()
	{
		float frTime = Time.timeSinceLevelLoad * Frequency;
		float sin = Mathf.Sin(frTime);
		sin = BobCurve.Evaluate(sin);

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

	private void Sway()
	{
		float lookX = _cmd.MouseLook.x;
		float lookY = _cmd.MouseLook.y;

		Vector3 leuler = SwayTransform.localEulerAngles;

		leuler.x += lookY * SwayVertSens;
		leuler.y += lookX * SwayHorzSens;
		leuler.z = 0;

		float lerpRate = Time.deltaTime * SwaybackRate;
		leuler.x = Mathf.LerpAngle(leuler.x, 0f, lerpRate);
		leuler.y = Mathf.LerpAngle(leuler.y, 0f, lerpRate);


		SwayTransform.localEulerAngles = leuler;
	}
}