using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;

public class PawnViewRigFPSTPS : PawnViewRigRotation
{
	[Header("Objects")]
	public GameObject ViewFirst;
	public GameObject ViewThird;
	public CinemachineVirtualCamera VcamFPS;
	public CinemachineVirtualCamera VcamTPS;
	public Transform FirstPersonViewModelParent;
	public Transform ThirdPersonPanParent;
	[Header("Settings")]
	public float ThirdPersonDistance = 2.75f;
	public float ThirdPersonHeight = 1f;
	public float ThirdPersonPan = 0f;
	public float ThirdPersonRaycastRadius = 0.5f;
	public LayerMask ThirdPersonRaycastLayer;
	[Header("State")]
	public ViewType View;

	private PawnRenderer _renderer;
	private ViewType _lastViewType;
	private Transform _thirdPersonDistanceTransform;
	private Transform _thirdPersonOriginTransform;


	public override bool ShouldShowSelfRenderer()
	{
		return View == ViewType.THIRD_PERSON;
	}

	public override Transform FetchAimTransform()
	{
		return View switch
		{
			ViewType.FIRST_PERSON => VcamFPS.transform,
			ViewType.THIRD_PERSON => VcamTPS.transform,
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	protected override void Initialize()
	{
		base.Initialize();
		_renderer = GetComponent<PawnRenderer>();
		_thirdPersonOriginTransform = ViewThird.transform;
		_thirdPersonDistanceTransform = ViewThird.transform.GetChild(0);
		ChangeView();
	}

	private void OnValidate()
	{
#if UNITY_EDITOR
		ChangeView();
#endif
	}

	public override void Tick()
	{
		base.Tick();

		CheckForChanges();
		AdjustDistance();
		CollisionCheck();
		

		void CheckForChanges()
		{
			if (View != _lastViewType)
			{
				ChangeView();
			}

			_lastViewType = View;
		}

		void AdjustDistance()
		{
			if (!IsOwner()) return;

			if (View != ViewType.THIRD_PERSON) return;

			float dist = Mathf.Abs(ThirdPersonDistance) * -1;

			Vector3 pos = _thirdPersonDistanceTransform.localPosition;
			pos.z = dist;
			_thirdPersonDistanceTransform.localPosition = pos;

			Vector3 panPos = ThirdPersonPanParent.localPosition;
			panPos.x = ThirdPersonPan;
			ThirdPersonPanParent.localPosition = panPos;
		}

		void CollisionCheck()
		{
			if (View != ViewType.THIRD_PERSON) return;

			Vector3 origin = _thirdPersonOriginTransform.position;
			Vector3 direction = _thirdPersonDistanceTransform.forward * -1;

			RaycastHit rhit;
			bool raycast = Physics.SphereCast(origin, ThirdPersonRaycastRadius, direction, out rhit, ThirdPersonDistance - ThirdPersonRaycastRadius, ThirdPersonRaycastLayer);
			if (raycast)
			{
				Vector3 directionToHitPoint = rhit.point - _thirdPersonOriginTransform.position;
				Vector3 projected = Vector3.Project(directionToHitPoint, direction);
				float projectedDistance = projected.magnitude;
				Vector3 pos = _thirdPersonDistanceTransform.localPosition;
				pos.z = -projectedDistance;
				_thirdPersonDistanceTransform.localPosition = pos;
			}
		}
	}

	private void ChangeView()
	{
		ViewFirst.SetActive(View == ViewType.FIRST_PERSON);
		ViewThird.SetActive(View == ViewType.THIRD_PERSON);

		if (Application.isPlaying == false) return;

		if (View == ViewType.THIRD_PERSON)
		{
			SetChaseOverrideTransform(_renderer.TransformRenderer, Vector3.up * ThirdPersonHeight);
		}
		else
		{
			SetChaseOverrideTransform(null, Vector3.zero);
		}
	}
}