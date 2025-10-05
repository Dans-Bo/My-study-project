using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 玩家跳跃状态
/// </summary>
public class PlayerJumpState : BaseState
{
    private Player player;
    //private bool hasJumped;
    public PlayerJumpState(Player player)
    {
        this.player = player;

    }
    public void OnEnter()
    {
        player.rigid2D.gravityScale = player.gravityScale;
        // hasJumped = false;

        /* if (player.JumpHandler.CanJump())
        {
            player.JumpHandler.ExecuteJump();
            //hasJumped = true;
            player.InputBufferSyetem.ClearBuffer("Jump"); //跳跃成功清除跳跃缓冲
        } */
    }


    public void OnUpdate()
    {
        
        PlayerJump();
        //TransformJumpGravity();
        player.Animator.Play("Jump", 0);
        
        if (player.PhysicsCheck.isGround && player.PlayerDirection.magnitude < 0.1f)
        {
            player.TransitionState(PlayerStateType.Idle);
        }
        if (player.PhysicsCheck.isGround && player.PlayerDirection.magnitude > 0.1f)
        {
            player.TransitionState(PlayerStateType.Move);
        }


    }
    public void OnFixedUpdate()
    {
        player.Move();
    }

    public void OnExit()
    {
        player.rigid2D.gravityScale = player.gravityScale;
    }

    /* private void TransformJumpGravity()
    {
      

    } */

    private void PlayerJump()
    {
       /*  if (player.PhysicsCheck.isGround)
        {
            player.coyouteTimeCounter = player.coyouteTime;
        }
        else
        {
            player.coyouteTimeCounter -= Time.deltaTime;
        } */
       /*  if (player.jumpBufferTimeCount > 0)
        {
            player.jumpBufferTimeCount -= Time.deltaTime;
        } */

        if (player.jumpBufferTimeCount > 0)
        {
            if (player.coyouteTimeCounter > 0f || player.PhysicsCheck.isGround)
            {
                player.rigid2D.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
                player.coyouteTimeCounter = 0f;
                player.jumpBufferTimeCount = 0f;
                player.pressedJump = false;
            }
        }

        if (player.rigid2D.velocity.y > 0)
        {
            player.rigid2D.gravityScale = player.gravityScale;
        }
        else if (player.rigid2D.velocity.y < 0)
        {
            player.rigid2D.gravityScale = player.fallGravityScale;
        }
    }

   /*  public bool CanTransitionTo(PlayerStateType type)
        {
           return type switch
            {
                PlayerStateType.Idle => player.Rigidbody2D.velocity.magnitude < 0.1f && player.PhysicsCheck.isGround && player.Rigidbody2D.velocity.y <=0 ,
                PlayerStateType.Move => player.PlayerDirection.magnitude > 0.1f && player.PhysicsCheck.isGround && player.Rigidbody2D.velocity.y <=0 ,
                _ => false,
            };
        } */
}
