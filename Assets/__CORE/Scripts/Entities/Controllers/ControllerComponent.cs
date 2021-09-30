using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerComponent : NetworkThing
{
	protected PawnControllerBase _controller { get; private set; }

	public void RegisterController(PawnControllerBase c)
	{
		_controller = c;
	}

	public abstract void OnInit(bool isOwner);

	public abstract void OnTick(bool isOwner);
}