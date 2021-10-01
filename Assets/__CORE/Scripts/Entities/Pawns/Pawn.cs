using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Networking.Types;

public class Pawn : NetworkThing
{
	[Header("Objects")]
	public GameObject OwnerOnlyObject;
	public PawnControllerBase CurrentController { get; private set; }

	private NetworkIdentity _nid;
	private PawnComponent[] _components;
	private IMouseLockable[] _mouseLockables;
	private PawnCommand _currentCommand;

	private void Awake()
	{
		OwnerOnlyObject.SetActive(false);
		_components = GetComponents<PawnComponent>();
		_mouseLockables = GetComponents<IMouseLockable>();
		_nid = GetComponent<NetworkIdentity>();

		foreach (PawnComponent c in _components)
		{
			c.Initialize(this);
		}
	}

	public bool HasAuthority()
	{
		return _nid.hasAuthority;
	}

	private void Update()
	{
		if (_nid.hasAuthority) OwnerUpdate();

		foreach (PawnComponent c in _components)
		{
			if (c.IsEnabled)
				c.Tick();
		}
	}

	private void FixedUpdate()
	{
		foreach (PawnComponent c in _components)
		{
			if (c.IsEnabled)
				c.FixedTick();
		}
	}

	private void LateUpdate()
	{
		foreach (PawnComponent c in _components)
		{
			if (c.IsEnabled)
				c.LateTick();
		}
	}

	public void OnPossessionBegin()
	{
		OwnerOnlyObject.SetActive(true);
		foreach (PawnComponent c in _components)
		{
			c.OnPossessionBegin();
		}
	}

	public void OnPossessionEnd()
	{
		OwnerOnlyObject.SetActive(false);
		foreach (PawnComponent c in _components)
		{
			c.OnPossessionEnded();
		}
	}

	public void AssignController(PawnControllerBase c)
	{
		CurrentController = c;
	}

	private void OwnerUpdate()
	{
		SendInputCommandToComponents();
	}

	public void SendInputCommand(PawnCommand cmd)
	{
		_currentCommand = cmd;
	}

	private void SendInputCommandToComponents()
	{
		foreach (PawnComponent c in _components)
		{
			c.SendInputCommand(_currentCommand);
		}

		_currentCommand = new PawnCommand();
	}

	public bool ShouldShowMouse()
	{
		if (_mouseLockables == null) return false;
		foreach (IMouseLockable m in _mouseLockables)
		{
			if (m.ShouldShowMouse()) return true;
		}
		return false;
	}
}