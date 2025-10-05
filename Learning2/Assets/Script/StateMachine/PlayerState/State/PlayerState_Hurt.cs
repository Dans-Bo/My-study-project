using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Hurt", fileName = "PlayerState_Hurt")]
public class PlayerState_Hurt : PlayerState
{
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        if (IsAnimationFinished)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
    override public void Exit()
    {
        playerController.IsHurt = false;
    }
}
