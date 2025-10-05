using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle", fileName = "PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    [SerializeField] float deceleration = 15f;
    public override void Enter()
    {
        base.Enter();
        playerController.CanAirJump = true;
        currentSpeed = playerController.MoveSpeed;

        //playerController.SetVelocityX(0f);
    }

    public override void Update()
    {
        if (playerInput.IsMove)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Move));
        }
        if (playerInput.HasJumpInputBuffer || playerInput.IsJump)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Jump));
        }

        if (!playerController.IsGround)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Fall));
        }

        if (playerInput.IsAttack)
        {
            switch (playerController.AttackCombo)
            {
                case 1:
                    playerStateMachine.SwitchState(typeof(PlayerState_NormalAttack_1));
                    break;
                case 2:
                    playerStateMachine.SwitchState(typeof(PlayerState_NormalAttack_2));
                    break;
                case 3:
                    playerStateMachine.SwitchState(typeof(PlayerState_NormalAttack_3));
                    break;
            }
        }
        if (playerController.IsHurt)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Hurt));
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
    }

    public override void FixedUpadte()
    {
        playerController.SetVelocityX(currentSpeed * playerController.transform.localScale.x);
    }
}
