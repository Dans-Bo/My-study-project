using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Died", fileName = "Died")]
public class Minotaur_Died :Minotaur_State
{
    bool isDestory = false;
    public override void Enter()
    {
        base.Enter();
        isDestory = false;
        rb2D.velocity = Vector2.zero;
        //rb2D.isKinematic = true; // 关闭物理模拟
        //transform.GetComponent<Collider2D>().enabled = false; // 禁用碰撞 

        //停用事件
        DisableEvents();


        animator.SetTrigger("Died");
        //TODO 音效

    }

    public override void Update()
    {
        if(IsAnimationFinished && !isDestory)
        {
            isDestory = true;
            Destroy(stateMachine.gameObject , 0.2f);
        }
    }
}
