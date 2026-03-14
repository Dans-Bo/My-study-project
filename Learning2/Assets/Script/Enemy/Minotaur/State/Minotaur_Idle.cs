
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Idle", fileName = "Idle")]
public class Minotaur_Idle :Minotaur_State
{
   
    public override void Enter()
    {
        base.Enter();

        minotaur_Agent.SetIdleTime();
        minotaur_Agent.StopMove();

        //TODO 后续迁移至animator manager   
        animator.SetTrigger("idle");
        
    }

    public override void Update()
    {
        base.Update();

        SwitchState();

    }

    private void SwitchState()
    {
        
         if(minotaur_Agent.IsDied)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Died);
            return;
        } 
        if(minotaur_Agent.IsHurt)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Hurt);
            return;
        }

        if(minotaur_Agent.IsChase)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Chase);
            return;
        }

        if(minotaur_Agent.IsAttack)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Attack);
            return;
        } 

        if(minotaur_Agent.IsIdleTimerExpired())
        {
            stateMachine.SwitchMinotaurState(EnemyState.Partol);
            return;
        }  
    }

}
