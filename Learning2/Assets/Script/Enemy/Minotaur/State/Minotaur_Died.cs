using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Died", fileName = "Died")]
public class Minotaur_Died :Minotaur_State
{
    public override void Enter()
    {
        base.Enter();
        
        minotaur_Agent.StopStartDestory();

        animator.SetTrigger("Died");
        //TODO 音效

    }

    public override void Update()
    {
        if(IsAnimationFinished && !minotaur_Agent.IsDestory)
        {
            minotaur_Agent.DestroyObject();
        }
    }
}
