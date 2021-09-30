using System.Collections;
using System.Collections.Generic;
using CMF;
using UnityEngine;

public class PawnMotorInputRelay : CharacterInput
{

    private PawnCommand _cmd;
    
    public void SetCommand(PawnCommand cmd)
    {
        _cmd = cmd;
    }
    
    public override float GetHorizontalMovementInput()
    {
        return _cmd.Movement.x;
    }

    public override float GetVerticalMovementInput()
    {
        return _cmd.Movement.y;
    }

    public override bool IsJumpKeyPressed()
    {
        return _cmd.Jump;
    }
}
