
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Idle", fileName = "Idle")]
public class Minotaur_Idle :Minotaur_State
{
    private float idleTime;  //站立时间
    private float startTime;
    private bool isTimeCount = false;
    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
        
        animator.SetTrigger("idle");
        
        idleTime = Random.Range(0.5f,4f);

        isTimeCount = true;
    }

    public override void Update()
    {
        base.Update();

        SwitchState();

    }

    private void SwitchState()
    {


        if(isChase)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Chase);
        }

        if(isTimeCount)
        {
            float time = Time.time - startTime;

            if(time  > idleTime)
            {
                isTimeCount = false;
                stateMachine.SwitchMinotaurState(EnemyState.Partol);
            }
        } 

        if(isAttack)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Attack);
        }

    }


    public override void Exit()
    {
        isTimeCount = false;
    }
}
