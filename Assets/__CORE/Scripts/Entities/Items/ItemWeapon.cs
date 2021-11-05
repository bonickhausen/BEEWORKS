using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ItemWeapon : ItemHoldable
{
	[Header("Weapon Setup")]
	public Parenter ViewModel;
	public Parenter WorldModel;
	public WeaponMechanismBase PrimaryMechanism;
	public WeaponMechanismBase SecondaryMechanism;
	[Header("Weapon Settings")]
	[Range(1, 9)]
	public uint Slot;

	public PawnWeaponHandler CurrentHolder { get; private set; }

	private bool _hasPrimaryMechanism;
	private bool _hasSecondaryMechanism;

	private void Awake()
	{
		_hasPrimaryMechanism = PrimaryMechanism;
		_hasSecondaryMechanism = SecondaryMechanism;
	}

	public void SetHolder(PawnWeaponHandler wh)
	{
		CurrentHolder = wh;
	}

	protected override void Tick()
	{
		base.Tick();

		EvaluateViewmodelVisibility();
		EvaluateWorldmodelVisibility();
	}

	private void EvaluateViewmodelVisibility()
	{
		bool shouldBeVisible = ViewModel.IsAttached && ViewModel.Target.gameObject.activeInHierarchy;

		ViewModel.gameObject.SetActive(shouldBeVisible);
	}

	private void EvaluateWorldmodelVisibility()
	{
		bool shouldBeVisible = WorldModel.IsAttached && WorldModel.Target.gameObject.activeInHierarchy;

		WorldModel.gameObject.SetActive(shouldBeVisible);

		if (shouldBeVisible)
		{
			WorldModel.SetPositionOffset(WorldModel.Target.TransformVector(PositionOffset));
		}
	}

	public void AttachViewModelToTransform(Transform t)
	{
		ViewModel.AttachToTransform(t);
	}

	public void AttachWorldModelToTransform(Transform t)
	{
		WorldModel.AttachToTransform(t);
	}

	public void TryAttack()
	{
		if (!_hasPrimaryMechanism) return;
		PrimaryMechanism.TryUse();
	}

	public void TryAltAttack()
	{
		if (!_hasSecondaryMechanism) return;
		SecondaryMechanism.TryUse();
	}
}