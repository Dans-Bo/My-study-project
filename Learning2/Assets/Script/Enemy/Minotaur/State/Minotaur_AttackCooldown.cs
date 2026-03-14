using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/AttackCooldown", fileName = "AttackCooldown")]
public class Minotaur_AttackCooldown : Minotaur_State
{
    [SerializeField] float attackCooldown = 1f;

    public override void Enter()
    {
        base.Enter();

        minotaur_Agent.SetAttackCoolDownTime();

        animator.SetTrigger("idle");
    }

    public override void Update()
    {
        base.Update();
        
        if(minotaur_Agent.IsAttackCoolDownExpired(attackCooldown))
        {
           
            SwitchState();
            
        }
    }

    private void SwitchState()
    {
        if(minotaur_Agent.IsHurt)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Hurt);
            return;
        }
        
        if(minotaur_Agent.IsAttack)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Attack);
            return;
        }
        else
        {
            stateMachine.SwitchMinotaurState(EnemyState.Idle);
            return;
        } 
    }


}
