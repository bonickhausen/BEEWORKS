using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(PositionConstraint))]
[RequireComponent(typeof(RotationConstraint))]
[RequireComponent(typeof(ScaleConstraint))]
public class Parenter : MonoBehaviour
{
	public Transform Target { get; private set; }
	public bool IsAttached { get; private set; }

	private PositionConstraint _pConst;
	private RotationConstraint _rConst;
	private ScaleConstraint _sConst;
	private bool _hasCachedComponents = false;

	private void Awake()
	{
		CacheComponents();
	}

	private void CacheComponents()
	{
		if (_hasCachedComponents) return;

		_pConst = GetComponent<PositionConstraint>();
		_rConst = GetComponent<RotationConstraint>();
		_sConst = GetComponent<ScaleConstraint>();

		_hasCachedComponents = true;
	}

	public void AttachToTransform(Transform t)
	{
		if (!_hasCachedComponents) CacheComponents();
		Target = t;
		IsAttached = Target != null;

		ConstraintSource cs = new() {sourceTransform = Target, weight = 1f};

		ClearAllConstraints();
		AssignAllConstraints();

		void ClearAllConstraints()
		{
			for (int i = 0; i < _pConst.sourceCount; i++)
			{
				_pConst.RemoveSource(0);
			}
			for (int i = 0; i < _rConst.sourceCount; i++)
			{
				_rConst.RemoveSource(0);
			}
			for (int i = 0; i < _sConst.sourceCount; i++)
			{
				_sConst.RemoveSource(0);
			}
		}

		void AssignAllConstraints()
		{
			_pConst.AddSource(cs);
			_rConst.AddSource(cs);
			_sConst.AddSource(cs);

			_pConst.translationOffset = Vector3.zero;
			_pConst.weight = 1f;
			_pConst.constraintActive = true;
			_rConst.rotationOffset = Vector3.zero;
			_rConst.weight = 1f;
			_rConst.constraintActive = true;
			_sConst.scaleOffset = Vector3.one;
			_sConst.weight = 1f;
			_sConst.constraintActive = true;
		}
	}

	public void SetPositionOffset(Vector3 pos)
	{
		if (!_hasCachedComponents) CacheComponents();
		_pConst.translationOffset = pos;
	}
}