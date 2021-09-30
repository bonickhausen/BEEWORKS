using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PawnInputBase : MonoBehaviour
{
	public abstract PawnCommand GetCommand();
}