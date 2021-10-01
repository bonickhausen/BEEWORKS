using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInteraction : PawnComponent
{
	public LayerMask Mask;
	public float InteractionDistance = 2f;

	public CORE_Delegates.VoidDelegate Evnt_OnInteractableChanged;
	
	private IInteractable _currentInteractable;
	private IInteractable _interactableLastFrame;
	private PawnViewBase _view;
	private float _lastUseTime;

	private const float USE_TIME_INTERVAL = 0.1f;

	protected override void Initialize()
	{
		base.Initialize();
		_view = _pawn.GetComponent<PawnViewBase>();
	}

	public IInteractable GetCurrentInteractable()
	{
		return _currentInteractable;
	}

	public override void Tick()
	{
		if (IsOwner())
		{
			FetchInteractable();	
		}

		if (_currentInteractable != _interactableLastFrame)
		{
			OnInteractableChanged();	
		}

		_interactableLastFrame = _currentInteractable;
		
		if (_cmd.Interact)
		{
			TryInteract();
		}
	}

	private void OnInteractableChanged()
	{
		Evnt_OnInteractableChanged?.Invoke();
	}

	private void FetchInteractable()
	{
		Transform aimTransform = _view.FetchAimTransform();
		Vector3 origin = aimTransform.position;
		Vector3 direction = aimTransform.forward;
		bool hit = Physics.Raycast(origin, direction, out RaycastHit rhit, InteractionDistance, Mask);
		_currentInteractable = null;
		if (hit)
		{
			IInteractable i = rhit.collider.GetComponentInParent<ItemBase>();
			if (i != null)
			{
				_currentInteractable = i;
			}
		}
	}

	private void TryInteract()
	{
		if (_currentInteractable == null) return;

		if (!_currentInteractable.CanUse(_pawn)) return;

		if (Time.timeSinceLevelLoad < _lastUseTime + USE_TIME_INTERVAL) return;

		_currentInteractable.Use(_pawn);

		_currentInteractable = null;

		_lastUseTime = Time.timeSinceLevelLoad;
	}
}