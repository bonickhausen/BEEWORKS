using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using UnityEngine;

public class PawnInputSimple : PawnInputBase
{
	public override PawnCommand GetCommand()
	{
		PawnCommand cmd = new();

		if (DebugLogManager.Instance.IsLogWindowVisible) return cmd;

		bool up = Input.GetKey(KeyCode.W);
		bool down = Input.GetKey(KeyCode.S);
		bool left = Input.GetKey(KeyCode.A);
		bool right = Input.GetKey(KeyCode.D);

		Vector2 wishVector = new();
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
		cmd.Attack = Input.GetMouseButton(0);
		cmd.AltAttack = Input.GetMouseButton(1);
		cmd.Interact = Input.GetKeyDown(KeyCode.E);
		cmd.Jump = Input.GetKey(KeyCode.Space);

		if (Input.GetKey(KeyCode.Alpha1)) cmd.SlotNumber = 1;
		if (Input.GetKey(KeyCode.Alpha2)) cmd.SlotNumber = 2;
		if (Input.GetKey(KeyCode.Alpha3)) cmd.SlotNumber = 3;
		if (Input.GetKey(KeyCode.Alpha4)) cmd.SlotNumber = 4;
		if (Input.GetKey(KeyCode.Alpha5)) cmd.SlotNumber = 5;
		if (Input.GetKey(KeyCode.Alpha6)) cmd.SlotNumber = 6;
		if (Input.GetKey(KeyCode.Alpha7)) cmd.SlotNumber = 7;
		if (Input.GetKey(KeyCode.Alpha8)) cmd.SlotNumber = 8;
		if (Input.GetKey(KeyCode.Alpha9)) cmd.SlotNumber = 9;
		if (Input.GetKey(KeyCode.Alpha0)) cmd.SlotNumber = 10;

			float mX = Input.GetAxis("Mouse X");
		float mY = Input.GetAxis("Mouse Y");

		cmd.MouseLook = new Vector2(mX, mY);


		return cmd;
	}
}