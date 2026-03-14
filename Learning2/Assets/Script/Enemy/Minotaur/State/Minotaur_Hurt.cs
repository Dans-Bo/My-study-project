using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Hurt", fileName = "Hurt")]
public class Minotaur_Hurt :Minotaur_State
{
    public override void Enter()
    {
        base.Enter();
        
        animator.SetTrigger("hurt");

        minotaur_Agent.Knockback();
        Debug.Log($"进入受伤状态");
        //TODO 音效播放
    }

    public override void Update()
    {
        if(IsAnimationFinished)
        {
             if(minotaur_Agent.IsDied)
            {
                stateMachine.SwitchMinotaurState(EnemyState.Died);
                return;
            }
            else if(minotaur_Agent.IsAttack)
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

    public override void Exit()
    {
        base.Exit();
        minotaur_Agent.StopHurt();
    }
}
