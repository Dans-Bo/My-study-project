using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Hurt", fileName = "Hurt")]
public class Minotaur_Hurt :Minotaur_State
{
    public override void Enter()
    {
        base.Enter();
        rb2D.velocity = Vector2.zero;
        OnHurtMoveBack();

        animator.SetTrigger("hurt");
        Debug.Log($"进入受伤状态");
        //TODO 音效播放
    }

    public override void Update()
    {
        if(IsAnimationFinished)
        {
            
            if(isDied)
            {
                stateMachine.SwitchMinotaurState(EnemyState.Died);
            }
            else
            {
                stateMachine.SwitchMinotaurState(EnemyState.Idle);
            }
            
        }
    }

    private void OnHurtMoveBack()
    {
        Vector2 direction = this.transform.position - attackerTransform.position;

        direction.y = 0;
        if(direction.sqrMagnitude < 0.01f) //该向量的平方长度,高效判断向量是否为零向量
        {
            direction = transform.right;
        }
        else
        {
            direction.Normalize();
        }

        rb2D.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        base.Exit();

        ResetHurtState();
        rb2D.velocity = Vector2.zero;
    }
}
