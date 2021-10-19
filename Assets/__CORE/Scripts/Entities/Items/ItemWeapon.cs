using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ItemWeapon : ItemHoldable
{
	[Header("Weapon Setup")]
	public PositionConstraint VModelPosConstraint;
	public RotationConstraint VModelRotConstraint;
	public ScaleConstraint VModelScaConstraint;
	public PositionConstraint WModelPosConstraint;
	public RotationConstraint WModelRotConstraint;
	public ScaleConstraint WModelScaConstraint;
	[Header("Weapon Settings")]
	[Range(1, 9)]
	public uint Slot;
	public int Damage;
	public float AttackInterval;

	private Transform _attachedViewmodelTransform;
	private GameObject _viewModelObject;
	private Transform _attachedWorldmodelTransform;
	private GameObject _worldModelObject;

	private void Awake()
	{
		_viewModelObject = VModelPosConstraint.gameObject;
		_worldModelObject = WModelPosConstraint.gameObject;
	}

	protected override void Tick()
	{
		base.Tick();

		EvaluateViewmodelVisibility();
		EvaluateWorldmodelVisibility();
	}

	private void EvaluateViewmodelVisibility()
	{
		bool shouldHide = _attachedViewmodelTransform == null || _attachedViewmodelTransform.gameObject.activeInHierarchy == false;
		
		_viewModelObject.SetActive(!shouldHide);
	}

	private void EvaluateWorldmodelVisibility()
	{
		bool shouldHide = _attachedWorldmodelTransform == null || _attachedWorldmodelTransform.gameObject.activeInHierarchy == false;

		_worldModelObject.SetActive(!shouldHide);
		
		if (!shouldHide)
		{
			WModelPosConstraint.translationOffset = _attachedWorldmodelTransform.TransformVector(PositionOffset);
		}
	}

	public void AttachWorldModelToTransform(Transform t)
	{
		_attachedWorldmodelTransform = t;

		ConstraintSource cs = new() {sourceTransform = t, weight = 1f};

		ClearAllConstraints();
		AssignAllConstraints();
		WModelPosConstraint.gameObject.SetActive(true);

		void ClearAllConstraints()
		{
			for (int i = 0; i < WModelPosConstraint.sourceCount; i++)
			{
				WModelPosConstraint.RemoveSource(0);
			}
			for (int i = 0; i < WModelRotConstraint.sourceCount; i++)
			{
				WModelRotConstraint.RemoveSource(0);
			}
			for (int i = 0; i < WModelScaConstraint.sourceCount; i++)
			{
				WModelScaConstraint.RemoveSource(0);
			}
		}

		void AssignAllConstraints()
		{
			WModelPosConstraint.AddSource(cs);
			WModelRotConstraint.AddSource(cs);
			WModelScaConstraint.AddSource(cs);

			WModelPosConstraint.translationOffset = Vector3.zero;
			WModelRotConstraint.rotationOffset = Vector3.zero;
			WModelScaConstraint.scaleOffset = Vector3.one;
			WModelPosConstraint.weight = 1f;
			WModelRotConstraint.weight = 1f;
			WModelScaConstraint.weight = 1f;
			WModelPosConstraint.constraintActive = true;
			WModelRotConstraint.constraintActive = true;
			WModelScaConstraint.constraintActive = true;
		}
	}

	public void AttachViewModelToTransform(Transform t)
	{
		_attachedViewmodelTransform = t;

		ConstraintSource cs = new() {sourceTransform = t, weight = 1f};

		ClearAllConstraints();
		AssignAllConstraints();
		VModelPosConstraint.gameObject.SetActive(true);

		void ClearAllConstraints()
		{
			for (int i = 0; i < VModelPosConstraint.sourceCount; i++)
			{
				VModelPosConstraint.RemoveSource(0);
			}
			for (int i = 0; i < VModelRotConstraint.sourceCount; i++)
			{
				VModelRotConstraint.RemoveSource(0);
			}
			for (int i = 0; i < VModelScaConstraint.sourceCount; i++)
			{
				VModelScaConstraint.RemoveSource(0);
			}
		}

		void AssignAllConstraints()
		{
			VModelPosConstraint.AddSource(cs);
			VModelRotConstraint.AddSource(cs);
			VModelScaConstraint.AddSource(cs);

			VModelPosConstraint.translationOffset = Vector3.zero;
			VModelRotConstraint.rotationOffset = Vector3.zero;
			VModelScaConstraint.scaleOffset = Vector3.one;
			VModelPosConstraint.weight = 1f;
			VModelRotConstraint.weight = 1f;
			VModelScaConstraint.weight = 1f;
			VModelPosConstraint.constraintActive = true;
			VModelRotConstraint.constraintActive = true;
			VModelScaConstraint.constraintActive = true;
		}
	}
}