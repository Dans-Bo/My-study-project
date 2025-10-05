using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/WallGrab", fileName = "PlayerState_WallGrab")]
public class PlayerState_WallGrab : PlayerState
{

    [SerializeField] float currentGravity;
    [SerializeField] float grabWallTime = 0.5f;
    [SerializeField] float grabWallSpeedY;
    [SerializeField] float moveSpeed;
    public override void Enter()
    {
        base.Enter();
        currentGravity = playerController.CurrentGravityScale;
        playerController.SetGravity(0f);
        playerController.SetVelocityY(grabWallSpeedY);
    }

    public override void Exit()
    {
        playerController.SetGravity(currentGravity);
    }

    public override void Update()
    {
        //base.Update();
        if (StateDuration > grabWallTime)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_WallSlide));
        }

        if (playerInput.IsJump)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_WallJump));
        }

    }

    public override void FixedUpadte()
    {
        playerController.Move(moveSpeed);
    }
   
}
