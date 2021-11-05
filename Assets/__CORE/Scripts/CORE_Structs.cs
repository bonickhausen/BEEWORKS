using UnityEngine;

public struct PawnCommand
{
	public Vector2 Movement;
	public Vector2 MouseLook;
	public bool Attack;
	public bool AltAttack;
	public bool Interact;
	public bool Jump;
	public short SlotNumber;
}