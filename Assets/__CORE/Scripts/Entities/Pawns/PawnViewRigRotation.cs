using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PawnViewRigRotation : PawnViewBase
{
	[Header("Objects")]
	public Transform YawTransform;
	public Transform PitchTransform;
	[Header("Settings")]
	public float HeightOffset;
	public float Sensitivity = 1f;

	protected float _yaw;
	protected float _pitch;

	private Transform _chaseOverride;
	private bool _hasChaseOverride;
	private Vector3 _chaseTargetOffset;


	public override void Tick()
	{
		CollectInput();
		MoveRig();
		RotateRig();
	}

	private void CollectInput()
	{
		_yaw += _cmd.MouseLook.x * Sensitivity;
		_yaw %= 360f;

		_pitch -= _cmd.MouseLook.y * Sensitivity;
		float limit = 79;
		_pitch = Mathf.Clamp(_pitch, -limit, limit);
	}

	private void MoveRig()
	{
		Vector3 targetPos = _motor.GetTransform().position + (Vector3.up * HeightOffset);

		if (_hasChaseOverride)
		{
			targetPos = _chaseOverride.position + _chaseOverride.TransformDirection(_chaseTargetOffset);
		}
		
		ViewRig.position = targetPos;
	}

	private void RotateRig()
	{
		YawTransform.localEulerAngles = new Vector3(0, _yaw, 0);
		PitchTransform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
	}

	protected void SetChaseOverrideTransform(Transform t, Vector3 offset)
	{
		_chaseOverride = t;
		_chaseTargetOffset = offset;
		_hasChaseOverride = (_chaseOverride != null);
	}
}