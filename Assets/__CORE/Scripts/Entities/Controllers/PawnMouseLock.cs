using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMouseLock : ControllerComponent
{
	public override void OnInit(bool isOwner)
	{ }

	public override void OnTick(bool isOwner)
	{
		if (isOwner)
		{
			CheckMouseState();
		}
	}

	private void CheckMouseState()
	{
		if (_controller.CurrentPawn && _controller.CurrentPawn.IsOwner())
		{
			bool showMouse = _controller.CurrentPawn.ShouldShowMouse();
			if (showMouse)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}
}