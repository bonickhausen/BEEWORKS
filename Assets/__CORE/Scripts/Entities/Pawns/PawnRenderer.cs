using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnRenderer : PawnComponent
{
	[Header("Objects")]
	public Transform TransformRenderer;
	[Header("Settings")]
	public float Height = 1f;
	public float LerpRate = 15f;
	public bool HideRendererForOwner = true;
	public RendererRotationType RotationType;


	private PawnMotor _motor;
	private PawnViewBase _view;
	private PawnViewRigRotation _viewRot;
	private bool _hasViewRot;

	private void Awake()
	{
		_motor = GetComponent<PawnMotor>();
		_view = GetComponent<PawnViewBase>();
		_viewRot = GetComponent<PawnViewRigRotation>();
		_hasViewRot = _viewRot != null;
	}

	public override void Tick()
	{
		MoveRenderer();
		RotateRenderer();
		ToggleVisibility();
	}

	private void ToggleVisibility()
	{
		if (!IsOwner()) return;
		if (HideRendererForOwner && !_view.ShouldShowSelfRenderer())
		{
			TransformRenderer.gameObject.SetActive(false);
		}
		else
		{
			TransformRenderer.gameObject.SetActive(true);
		}
	}

	private void MoveRenderer()
	{
		Vector3 localPos = TransformRenderer.localPosition;
		Transform motorTransform = _motor.GetTransform();
		float lerpRate = Time.deltaTime * LerpRate;
		Vector3 newPos = Vector3.Lerp(localPos, motorTransform.localPosition + (Vector3.up * Height), lerpRate);
		TransformRenderer.localPosition = newPos;
	}

	private void RotateRenderer()
	{
		Vector3 fw = TransformRenderer.forward;
		
		if (RotationType == RendererRotationType.FOLLOW_CAMERA && _hasViewRot)
		{
			fw = _viewRot.YawTransform.forward;
		}
		else
		{
			Vector3 dir = _motor.Direction;
			dir.y = 0f;
			fw = dir.normalized;
		}
		
		TransformRenderer.forward = Vector3.Lerp(TransformRenderer.forward, fw, Time.deltaTime * LerpRate);
	}
}