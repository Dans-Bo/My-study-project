using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 玩家移动状态
/// </summary>
public class PlayerMoveState : BaseState
{
    private Player player;

    public PlayerMoveState(Player player)
    {
        this.player = player;
    }
    public void OnEnter()
    {
        player.Animator.Play("Run", 0);
    }


    public void OnUpdate()
    {
        if (player.PhysicsCheck.isGround && player.PlayerDirection.magnitude < 0.1f)
        {
            player.TransitionState(PlayerStateType.Idle);
            return;
        }
        if (CanJump())
        {
            player.TransitionState(PlayerStateType.Jump);
            return;
        }

    }
    public void OnFixedUpdate()
    {
        player.Move();
    }

    public void OnExit()
    {

    }
    
    private bool CanJump()
    {
        bool hasJumpInput = player.pressedJump || player.jumpBufferTimeCount > 0;

        bool canJump = player.PhysicsCheck.isGround || player.coyouteTimeCounter > 0;

        return hasJumpInput && canJump;
    }

   /*  public bool CanTransitionTo(PlayerStateType type)
      {
          return type switch
          {
              PlayerStateType.Idle => player.Rigidbody2D.velocity.magnitude < 0.01f,
              PlayerStateType.Jump => player.InputBufferSyetem.IsBuffered("Jump") && player.JumpHandler.CanJump(),
              _ => false,
          };
      } */
}
