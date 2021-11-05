using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class PawnViewRigRotation : PawnViewBase
{
	[Header("Objects")]
	public Transform YawTransform;
	public Transform PitchTransform;
	[Header("Settings")]
	public float HeightOffset;
	public float Sensitivity = 1f;
	[Header("Networked State")]
	[SyncVar]
	public float NetworkedPitch;
	[SyncVar]
	public float NetworkedYaw;

	public const float ANGLE_LIMIT = 79;

	protected float _yaw;
	protected float _pitch;

	private Transform _chaseOverride;
	private bool _hasChaseOverride;
	private Vector3 _chaseTargetOffset;
	private float _lastOwnerNetworkedPitch;
	private float _lastOwnerNetworkedYaw;


	public override void Tick()
	{
		CollectInput();
		MoveRig();
		RotateRig();
		ReplicateStateToServer();

		void ReplicateStateToServer()
		{
			if (!IsOwner()) return;

			if (!CanSendRPC()) return;

			bool pitchChanged = false;
			bool yawChanged = false;

			if (Mathf.Approximately(_pitch, _lastOwnerNetworkedPitch) == false)
			{
				pitchChanged = true;
			}

			if (Mathf.Approximately(_yaw, _lastOwnerNetworkedYaw) == false)
			{
				yawChanged = true;
			}

			if (pitchChanged || yawChanged)
			{
				CmdSendPitchYaw(_pitch, _yaw);
			}

			_lastOwnerNetworkedPitch = _pitch;
			_lastOwnerNetworkedYaw = _yaw;
		}
	}
	
	

	private void CollectInput()
	{
		if (IsOwner())
		{
			_yaw += _cmd.MouseLook.x * Sensitivity;
			_yaw %= 360f;

			_pitch -= _cmd.MouseLook.y * Sensitivity;
			float limit = ANGLE_LIMIT;
			_pitch = Mathf.Clamp(_pitch, -limit, limit);	
		}
		else
		{
			float lerpRate = Time.deltaTime * 25f;
			_yaw = Mathf.Lerp(_yaw, NetworkedYaw, lerpRate);
			_pitch = Mathf.Lerp(_pitch, NetworkedPitch, lerpRate);
		}
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
	
	[Command]
	private void CmdSendPitchYaw(float pitch, float yaw)
	{
		NetworkedPitch = pitch;
		NetworkedYaw = yaw;
	}
}