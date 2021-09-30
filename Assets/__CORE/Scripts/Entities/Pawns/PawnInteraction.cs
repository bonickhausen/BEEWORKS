using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInteraction : PawnComponent
{
	public ColliderBroadcaster Trigger;

	public IInteractable _currentInteractable;

	private float _lastUseTime;

	private const float INTERACTION_TIME_INTERVAL = 1f;

	public override void OnPossessionBegin()
	{
		Trigger.TriggerStay += TriggerStay;
	}

	public override void OnPossessionEnded()
	{
		Trigger.TriggerStay -= TriggerStay;
	}

	private void TriggerStay(Collider other)
	{
		IInteractable interactable = other.GetComponentInParent<IInteractable>();

		if (interactable == null) return;

		ConsiderInteractable(interactable);
	}

	public override void Tick()
	{
		if (_cmd.Interact)
		{
			TryInteract();
		}
	}

	private void TryInteract()
	{
		if (_currentInteractable == null) return;

		if (!_currentInteractable.CanUse(_pawn)) return;

		_currentInteractable.Use(_pawn);

		_currentInteractable = null;

		_lastUseTime = Time.timeSinceLevelLoad;
	}

	private void ConsiderInteractable(IInteractable i)
	{
		if (Time.timeSinceLevelLoad <= _lastUseTime + INTERACTION_TIME_INTERVAL) return;

		if (_currentInteractable == null)
		{
			_currentInteractable = i;
		}
	}
}