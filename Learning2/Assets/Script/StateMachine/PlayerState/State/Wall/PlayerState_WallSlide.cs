using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/WallSlide", fileName = "PlayerState_WallSlide")]
public class PlayerState_WallSlide : PlayerState
{
    float currentGravity;
    [SerializeField] float slideGravity = 0.5f;
    public override void Enter()
    {
        base.Enter();
        
        currentGravity = playerController.CurrentGravityScale;
        playerController.SetGravity(slideGravity);
        playerController.SetVelocityY(0f);
    }
    public override void Exit()
    {
        playerController.SetGravity(currentGravity);
    }

    public override void Update()
    {
        if (playerController.IsGround)
        {
            if (playerInput.IsMove)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Move));
            }
            else
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Idle));
            }
        }
        else if (!playerController.IsWall)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Fall));
        }

        if (playerInput.IsJump)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_WallJump));
        }
    }
}
