using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Fall", fileName = "PlayerState_Fall")]
public class PlayerState_Fall : PlayerState
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] AnimationCurve speedCurve;
    public override void Update()
    {
        if (playerController.IsGround)
        {
            if (!playerInput.IsMove)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Idle));
            }
            else
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Move));
            }
        }

        if (playerInput.IsJump)
        {
            if (playerController.CanAirJump)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_AirJump));
                return;
            }

            playerInput.SetJumpInputBufferTime();
        }

        if (playerController.IsWall)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_WallSlide));
        }
        if (playerController.IsHurt)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Hurt));
        }
    }

    public override void FixedUpadte()
    {
        playerController.SetVelocityY(speedCurve.Evaluate(StateDuration));

        playerController.Move(moveSpeed);
    }
    
}
