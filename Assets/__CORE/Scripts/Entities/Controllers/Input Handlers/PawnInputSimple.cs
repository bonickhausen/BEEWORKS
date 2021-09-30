using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInputSimple : PawnInputBase
{
	public override PawnCommand GetCommand()
	{
		PawnCommand cmd = new PawnCommand();

		bool up = Input.GetKey(KeyCode.W);
		bool down = Input.GetKey(KeyCode.S);
		bool left = Input.GetKey(KeyCode.A);
		bool right = Input.GetKey(KeyCode.D);

		Vector2 wishVector = new Vector2();
		if (up)
		{
			wishVector.y = 1;
		}
		else if (down)
		{
			wishVector.y = -1;
		}
		if (left)
		{
			wishVector.x = -1;
		}
		else if (right)
		{
			wishVector.x = 1;
		}

		cmd.Movement = wishVector;
		cmd.Attack = Input.GetMouseButtonDown(0);
		cmd.Interact = Input.GetKeyDown(KeyCode.E);
		cmd.Jump = Input.GetKey(KeyCode.Space);

		float mX = Input.GetAxis("Mouse X");
		float mY = Input.GetAxis("Mouse Y");

		cmd.MouseLook = new Vector2(mX, mY);


		return cmd;
	}
}