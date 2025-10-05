using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Jump", fileName = "PlayerState_Jump")]
public class PlayerState_Jump : PlayerState
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 7f;
    //[SerializeField] ParticleSystem jumpVFX;
    //[SerializeField] AnimationCurve speedCurve;
    public override void Enter()
    {
        base.Enter();
        playerInput.HasJumpInputBuffer = false;
        playerController.SetVelocityY(jumpForce);
        //Instantiate(original: jumpVFX, position: playerController.transform.position, rotation: Quaternion.identity);
    }

    public override void Update()
    {
        if (playerInput.IsStopJump || playerController.IsFall)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Fall));
        }
        if (playerController.IsHurt)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Hurt));
        }
    }

    public override void FixedUpadte()
    {
        playerController.Move(moveSpeed);
    }

}
