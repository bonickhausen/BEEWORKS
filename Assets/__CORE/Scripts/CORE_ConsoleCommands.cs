using System;
using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using UnityEngine;

public class CORE_ConsoleCommands : MonoBehaviour
{
	private void Start()
	{
		/*DevConsole.singleton.AddCommand(new ActionCommand(ToggleThirdPerson)
		{
			alias = "cam_tp"
		});*/
	}

	[ConsoleMethod( "cam_tp", "Toggles third person view." ), UnityEngine.Scripting.Preserve]
	public static void ToggleThirdPerson()
	{
		PawnControllerPlayer player = PawnControllerPlayer.LocalInstance;
		if (player == null) return;

		Pawn pawn = player.CurrentPawn;
		if (pawn == null) return;

		PawnViewRigFPSTPS view = pawn.GetComponent<PawnViewRigFPSTPS>();
		if (view == null) return;

		view.View = view.View switch
		{
			ViewType.FIRST_PERSON => ViewType.THIRD_PERSON,
			ViewType.THIRD_PERSON => ViewType.FIRST_PERSON,
			_ => view.View
		};
	}
}