using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 角色攻击状态
/// </summary>
public class PlayerAttackState : BaseState

{
    private Player player;

    public PlayerAttackState(Player player)
    {
        this.player = player;
    }
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }


    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
    public void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

 /*    public bool CanTransitionTo(PlayerStateType type)
    {
        return type switch
        {
            PlayerStateType.Move => player.PlayerDirection != Vector2.zero,
            PlayerStateType.Jump => player.InputBufferSyetem.IsBuffered("Jump") && player.JumpHandler.CanJump(),
            _ => false,
        };
    }*/
} 
