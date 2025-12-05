using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/NormalAttack_1", fileName = "PlayerState_NormalAttack_1")]
public class PlayerState_NormalAttack_1 : PlayerState
{
    [SerializeField] float attackTimeStop = 2f;
    public override void Enter()
    {
        base.Enter();
        playerController.StartAttackTimer(attackTimeStop);
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_WeaponSwingLight1);
    }
    public override void Update()
    {
        if (IsAnimationFinished)
        {
            if (playerInput.IsAttack && playerController.AttackCombo == 1)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_NormalAttack_2));
            }

            if (playerInput.IsMove)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Move));
            }
            else
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Idle));
            }
        }

        if (playerController.IsHurt)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Hurt));
        }
        
    }

    public override void Exit()
    {      
        playerController.AttackCombo = 2;
    }
}
