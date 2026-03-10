using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Hurt", fileName = "PlayerState_Hurt")]
public class PlayerState_Hurt : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerController.OnHurtMoveBack();
        animator.SetTrigger("hurt");
        //Debug.Log("进入受伤状态");

    }
    public override void Update()
    {
        if (IsAnimationFinished)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Idle));
        }

        if(playerController.IsDie && IsAnimationFinished)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Died));
        }
    }
    override public void Exit()
    {
        playerController.IsHurt = false;
    }
}
