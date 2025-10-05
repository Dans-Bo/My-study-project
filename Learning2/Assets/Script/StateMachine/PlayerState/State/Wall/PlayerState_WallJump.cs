using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/WallJump", fileName = "PlayerState_WallJump")]
public class PlayerState_WallJump : PlayerState
{
    [SerializeField] float jumpForce_Y;
    [SerializeField] float jumpForce_X;


    public override void Enter()
    {
        base.Enter();

        playerController.CanAirJump = true;
        //playerController.SetVelocityY(jumpForce);
        playerController.SetVelocity(jumpForce_X * playerController.PlayerDirection * -1, jumpForce_Y);
        playerController.PlayerDirection *= -1;
        
    }

    public override void Update()
    {
        if (playerInput.IsJump)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_AirJump));
        } 
        if (playerController.IsFall)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Fall));
        }
    }

}
