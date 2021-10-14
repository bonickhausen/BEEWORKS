using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHoldable : ItemBase
{
	[Header("Holdable Objects")]
	public Transform LeftHandTransform;
	public Transform RightHandTransform;
	[Header("Holdable Settings")]
	public Vector3 PositionOffset;

	private void OnDrawGizmosSelected()
	{
		
	}
}