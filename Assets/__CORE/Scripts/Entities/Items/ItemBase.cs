using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class ItemBase : NetworkBehaviour, IInteractable
{
	[Header("Settings")]
	public string ItemName;
	[Header("State")]
	[SyncVar]
	public uint CurrentHolder;


	public bool CanUse(Pawn pawn)
	{
		return IsBeingHeld() == false;
	}

	public void Use(Pawn pawn)
	{
		Debug.LogError("AEIOU");
	}

	public string GetInteractionMessage()
	{
		return "Pick " + ItemName + "?";
	}

	public bool IsBeingHeld()
	{
		return CurrentHolder == 0;
	}
}