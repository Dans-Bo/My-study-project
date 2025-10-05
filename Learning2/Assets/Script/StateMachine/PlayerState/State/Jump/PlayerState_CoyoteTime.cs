using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/CoyoteTime", fileName = "PlayerState_CoyoteTime")]
public class PlayerState_CoyoteTime : PlayerState
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float acceleration = 15f;
    [SerializeField] float coyoteTime = 0.1f;

    public override void Enter()
    {
        base.Enter();
    
        playerController.SetGravity(0f);

    }

    public override void Exit()
    {
        playerController.SetGravity(8f);
    }

    public override void Update()
    {

        if (playerInput.IsJump)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Jump));
        }

        if (!playerController.IsGround)
        {
            if (!playerInput.IsMove || StateDuration > coyoteTime)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Fall));
            }
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, runSpeed, acceleration * Time.deltaTime);
    }

    public override void FixedUpadte()
    {
        //playerController.SetVelocityX(runSpeed); //直接跑动
        playerController.Move(runSpeed);
    }
}
