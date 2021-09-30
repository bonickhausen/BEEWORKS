using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnViewRigFPSTPS : PawnViewRigRotation
{
	[Header("Objects")]
	public GameObject ViewFirst;
	public GameObject ViewThird;
	[Header("Settings")]
	public float ThirdPersonDistance = 2.75f;
	public float ThirdPersonRaycastRadius = 0.5f;
	public LayerMask ThirdPersonRaycastLayer;
	[Header("State")]
	public ViewType View;

	private ViewType _lastViewType;
	private Transform _thirdPersonDistanceTransform;
	private Transform _thirdPersonOriginTransform;

	public override bool ShouldShowSelfRenderer()
	{
		return View == ViewType.THIRD_PERSON;
	}

	protected override void Initialize()
	{
		base.Initialize();
		_thirdPersonOriginTransform = ViewThird.transform;
		_thirdPersonDistanceTransform = ViewThird.transform.GetChild(0);
		ChangeView();
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
	}
}