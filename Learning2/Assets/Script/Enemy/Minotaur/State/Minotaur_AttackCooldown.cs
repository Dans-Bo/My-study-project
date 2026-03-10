using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/AttackCooldown", fileName = "AttackCooldown")]
public class Minotaur_AttackCooldown : Minotaur_State
{
    [SerializeField] float attackCooldown = 1f;
    private float startTime;
    private bool isTimeCount = false;

    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
        isTimeCount = true;

        animator.SetTrigger("idle");
    }

    public override void Update()
    {
        base.Update();
        
        if(isTimeCount)
        {
            bool timeOver = Time.time - startTime > attackCooldown;
            if(timeOver)
            {
                isTimeCount = false;
                SwitchState();
            }
        }
    }

    private void SwitchState()
    {
        if(isAttack)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Attack);
        }
        else
        {
            stateMachine.SwitchMinotaurState(EnemyState.Idle);
        }
    }


}
