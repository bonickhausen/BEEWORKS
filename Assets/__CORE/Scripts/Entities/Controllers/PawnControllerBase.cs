using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class PawnControllerBase : NetworkThing
{
	[SyncVar]
	public string ControllerName;
	[SyncVar]
	public uint CurrentPawnId;

	public Pawn CurrentPawn { get; private set; }

	protected ControllerComponent[] _components;

	private PawnInputBase _input;
	private uint _pawnIdLastFrame;

	protected abstract void CacheComponents();
	protected abstract void Initialize();

	private void Awake()
	{
		CacheComponents();
		FetchControllerComponents();
		Initialize();
	}

	private void FetchControllerComponents()
	{
		_input = GetComponent<PawnInputBase>();
		_components = GetComponentsInChildren<ControllerComponent>();
		for (int index = 0; index < _components.Length; index++)
		{
			ControllerComponent cc = _components[index];
			cc.RegisterController(this);
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		for (int index = 0; index < _components.Length; index++)
		{
			ControllerComponent cc = _components[index];
			cc.OnInit(IsOwner());
		}
	}

	private void Update()
	{
		TickComponents();
		SendInputCommand();
		CheckForChanges();
	}

	private void CheckForChanges()
	{
		if (CurrentPawnId != _pawnIdLastFrame)
		{
			OnPawnChanged();
		}

		_pawnIdLastFrame = CurrentPawnId;

		void OnPawnChanged()
		{
			if (CurrentPawn)
			{
				if (IsOwner()) CurrentPawn.OnPossessionEnd();
			}

			if (CurrentPawnId == 0)
			{
				CurrentPawn = null;
			}
			else
			{
				NetworkIdentity nid = FetchNetworkIdentity(CurrentPawnId);
				if (nid)
				{
					CurrentPawn = nid.GetComponent<Pawn>();
					CurrentPawn.AssignController(this);
					if (IsOwner()) CurrentPawn.OnPossessionBegin();
				}
			}
		}
	}

	private void TickComponents()
	{
		for (int index = 0; index < _components.Length; index++)
		{
			ControllerComponent cc = _components[index];
			cc.OnTick(IsOwner());
		}
	}

	private void SendInputCommand()
	{
		if (IsOwner() && CurrentPawn)
		{
			CurrentPawn.SendInputCommand(_input.GetCommand());
		}
	}
}