using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class ItemBase : NetworkThing, IInteractable
{
	[Header("Objects")]
	public GameObject PickupableObject;
	[Header("Settings")]
	public string ItemName;
	[Header("State")]
	[SyncVar]
	public uint CurrentHolder;

	private uint _holderLastFrame;

	public bool CanUse(Pawn pawn)
	{
		return IsBeingHeld() == false;
	}

	public void Use(Pawn pawn)
	{
		PawnInventory pi = pawn.GetComponent<PawnInventory>();
		pi.TryAddItemToInventory(this);
	}

	public string GetInteractionMessage()
	{
		return "Pick " + ItemName + "?";
	}

	public bool IsBeingHeld()
	{
		return CurrentHolder != 0;
	}

	private void Update()
	{
		if (CurrentHolder != _holderLastFrame)
		{
			OnHolderChanged();
		}

		_holderLastFrame = CurrentHolder;

		void OnHolderChanged()
		{
			TogglePickupableObject(CurrentHolder <= 0);
		}
		
		Tick();
	}

	protected virtual void Tick()
	{
		
	}

	public void TogglePickupableObject(bool isOn)
	{
		PickupableObject.SetActive(isOn);
	}
}