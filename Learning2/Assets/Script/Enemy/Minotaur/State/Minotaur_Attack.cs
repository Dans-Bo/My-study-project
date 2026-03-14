using System.Collections;
using BehaviourTrees;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Attack", fileName = "Attack")]
public class Minotaur_Attack : Minotaur_State
{
    public override void Enter()
    {
        base.Enter();

        minotaur_Agent.StopMove();

        animator.SetTrigger("attack"); //动画运行

        //isAttack = false;
        //TODO 播放音效
    }

    public override void Update()
    {
        base.Update();
        
        if(IsAnimationFinished) //攻击动画完成切换硬值状态，避免一直攻击
        {
            stateMachine.SwitchMinotaurState(EnemyState.AttackCooldown);
            return;
        } 
    } 
        
}
