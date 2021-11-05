using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class MuzzleFlasher : MonoBehaviour
{
	[Header("Objects")]
	public HDAdditionalLightData[] Lights;
	[Header("Light Settings")]
	public float LightLerpRate = 25f;

	private void Awake()
	{
		SetDimmer(0f);
	}

	public void Flash()
	{
		SetDimmer(1f);
	}

	private void LateUpdate()
	{
		LerpLight();

		void LerpLight()
		{
			float lerpRate = Time.deltaTime * LightLerpRate;
			SetDimmer(Mathf.Lerp(Lights[0].lightDimmer, 0f, lerpRate));
		}
	}

	private void SetDimmer(float f)
	{
		for (int index = 0; index < Lights.Length; index++)
		{
			HDAdditionalLightData l = Lights[index];
			l.SetLightDimmer(f);
		}
	}
}