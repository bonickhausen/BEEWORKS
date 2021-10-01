using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ItemWeapon : ItemHoldable
{
	[Header("Weapon Setup")]
	public PositionConstraint PosConstraint;
	public RotationConstraint RotConstraint;
	public ScaleConstraint ScaConstraint;
	[Header("Weapon Settings")]
	[Range(1, 9)]
	public uint Slot;
	public int Damage;
	public float AttackInterval;

	public void AttachViewModelToTransform(Transform t)
	{
		ConstraintSource cs = new() {sourceTransform = t, weight = 1f};

		ClearAllConstraints();
		AssignAllConstraints();
		PosConstraint.gameObject.SetActive(true);

		void ClearAllConstraints()
		{
			for (int i = 0; i < PosConstraint.sourceCount; i++)
			{
				PosConstraint.RemoveSource(0);
			}
			for (int i = 0; i < RotConstraint.sourceCount; i++)
			{
				RotConstraint.RemoveSource(0);
			}
			for (int i = 0; i < ScaConstraint.sourceCount; i++)
			{
				ScaConstraint.RemoveSource(0);
			}
		}

		void AssignAllConstraints()
		{
			PosConstraint.AddSource(cs);
			RotConstraint.AddSource(cs);
			ScaConstraint.AddSource(cs);

			PosConstraint.translationOffset = Vector3.zero;
			RotConstraint.rotationOffset = Vector3.zero;
			ScaConstraint.scaleOffset = Vector3.one;
			PosConstraint.weight = 1f;
			RotConstraint.weight = 1f;
			ScaConstraint.weight = 1f;
			PosConstraint.constraintActive = true;
			RotConstraint.constraintActive = true;
			ScaConstraint.constraintActive = true;
		}
	}
}