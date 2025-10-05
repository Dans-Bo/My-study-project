using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 玩家闲置状态
/// </summary>
public class PlayerIdleState : BaseState
{
    private Player player;

    public PlayerIdleState(Player player)
    {
        this.player = player;
    }
    public void OnEnter()
    {
        player.Animator.Play("Idle", 0);
    }


    public void OnUpdate()
    {
        if (CanJump())
        {
            player.TransitionState(PlayerStateType.Jump);
            return;
        }
        if (player.PhysicsCheck.isGround && player.PlayerDirection.magnitude > 0.1f)
        {
            player.TransitionState(PlayerStateType.Move);
            return;
        }

    }
    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

    /*   public bool CanTransitionTo(PlayerStateType type)
      {
          return type switch
          {
              PlayerStateType.Move => player.PlayerDirection != Vector2.zero,
              PlayerStateType.Jump => player.InputBufferSyetem.IsBuffered("Jump") && player.JumpHandler.CanJump(),
              _ => false,
          };
      } */

    private bool CanJump()
    {
        bool hasJumpInput = player.pressedJump || player.jumpBufferTimeCount > 0;

        bool canJump = player.PhysicsCheck.isGround || player.coyouteTimeCounter > 0;

        return hasJumpInput && canJump;
    }
}
