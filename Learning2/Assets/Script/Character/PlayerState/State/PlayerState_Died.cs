using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Died", fileName = "PlayerState_Died")]
public class PlayerState_Died : PlayerState
{
    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("isDied");
    }

    public override void Update()
    {
        if(IsAnimationFinished)
        {
            playerController.OnDiedDestory();
        }
    }
}
