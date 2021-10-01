using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PawnHUD : PawnComponent
{
	[Header("Interaction")]
	public TextMeshProUGUI TextInteraction;

	private PawnInteraction _interaction;

	protected override void Initialize()
	{
		base.Initialize();
		_interaction = GetComponent<PawnInteraction>();
		_interaction.Evnt_OnInteractableChanged += OnInteractableChanged;
		OnInteractableChanged();
	}

	private void OnInteractableChanged()
	{
		IInteractable i = _interaction.GetCurrentInteractable();
		if (i == null)
		{
			TextInteraction.text = String.Empty;
		}
		else
		{
			TextInteraction.text = i.GetInteractionMessage();
		}
	}
}