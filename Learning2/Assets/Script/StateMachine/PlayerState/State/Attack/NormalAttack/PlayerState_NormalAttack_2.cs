using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/NormalAttack_2", fileName = "PlayerState_NormalAttack_2")]
public class PlayerState_NormalAttack_2 : PlayerState
{
    [SerializeField] float attackTimeStop = 2f;
    public override void Enter()
    {
        base.Enter();
        playerController.StartAttackTimer(attackTimeStop);
    }
    public override void Update()
    {
         if (IsAnimationFinished)
        {
            if (playerInput.IsAttack && playerController.AttackCombo == 2)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_NormalAttack_3));
            }

            if (playerInput.IsMove)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Move));
            }
            else
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Idle));
            }
            
        }
        if (playerController.IsHurt)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Hurt));
        }    
    }

    public override void Exit()
    {
       playerController.AttackCombo = 3;
    }
}
