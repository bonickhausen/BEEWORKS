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


	private PawnMotor _motor;
	private PawnViewBase _view;

	private void Awake()
	{
		_motor = GetComponent<PawnMotor>();
		_view = GetComponent<PawnViewBase>();
	}

	public override void Tick()
	{
		MoveRenderer();
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
}