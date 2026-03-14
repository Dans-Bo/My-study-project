using UnityEngine;
using BehaviourTrees;

[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Chase", fileName = "Chase")]
public class Minotaur_Chase : Minotaur_State
{
    [SerializeField] private float chaseSpeed;

    public override void Update()
    {
        base.Update();

        minotaur_Agent.ChaseFaceDirection();
        SwitchState();

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        minotaur_Agent.ChaseMove(chaseSpeed);
    }

    private void SwitchState()
    {
        if(!minotaur_Agent.IsChase)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Idle);
            return;
        }

        if(minotaur_Agent.IsAttack)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Attack);
            return;
        } 
    }

    public override void Exit()
    {
        base.Exit();
        minotaur_Agent.StopChase();
    }


}
