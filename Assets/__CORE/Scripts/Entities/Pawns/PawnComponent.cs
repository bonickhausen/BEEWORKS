using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class PawnComponent : NetworkThing
{
	[Header("Component Settings")]
	public bool IsEnabled = true;

	protected Pawn _pawn;
	protected PawnCommand _cmd;

	public void Initialize(Pawn p)
	{
		_pawn = p;
		Initialize();
	}

	public void SendInputCommand(PawnCommand cmd)
	{
		_cmd = cmd;
	}

	protected virtual void Initialize()
	{ }

	public virtual void Tick()
	{ }

	public virtual void FixedTick()
	{ }

	public virtual void LateTick()
	{ }

	public virtual void OnPossessionBegin()
	{ }

	public virtual void OnPossessionEnded()
	{ }

	protected new bool IsOwner()
	{
		return _pawn.HasAuthority();
	}
}