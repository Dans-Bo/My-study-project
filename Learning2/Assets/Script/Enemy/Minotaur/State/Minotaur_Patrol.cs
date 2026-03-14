
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Partol", fileName = "Partol")]
public class Minotaur_Patrol:Minotaur_State
{
    [SerializeField] float partolSpeed; //巡逻速度


    public override void Enter()
    {
        base.Enter();
        minotaur_Agent.UpdateTargetWayPoint();
        
    }
        
    public override void Update()
    {
        base.Update();

        SwitchState();
        minotaur_Agent.Patrol();

        //animator.SetFloat("speed" , partolSpeed); 
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        minotaur_Agent.PartolMove(partolSpeed);
       
    }

    public override void Exit()
    {
        base.Exit();
        minotaur_Agent.StopMove();
    }
    
/// <summary>
/// 状态转换
/// </summary>
    private void SwitchState()
    {
        if(minotaur_Agent.IsAttack)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Attack); 
            return;
        }

        if(minotaur_Agent.IsChase)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Chase);
            return;
        }

        if(minotaur_Agent.ArrayWayPoint())
        {
            stateMachine.SwitchMinotaurState(EnemyState.Idle);
            return;
        }
    }

    

}
