using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Move", fileName = "PlayerState_Move")]
public class PlayerState_Move : PlayerState
{

    [SerializeField]float runSpeed = 10f;
    [SerializeField] float acceleration = 15f;
    public override void Enter()
    {
        base.Enter();
        playerController.CanAirJump = true;
        currentSpeed = playerController.MoveSpeed;
    }

    public override void Update()
    {
        if (!playerInput.IsMove)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Idle));
        }

        if (playerInput.HasJumpInputBuffer || playerInput.IsJump)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Jump));
        }

        if (!playerController.IsGround)
        {
            // playerStateMachine.SwitchState(typeof(PlayerState_CoyoteTime));
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
            currentSpeed = Mathf.MoveTowards(currentSpeed, runSpeed, acceleration * Time.deltaTime);
    }

    public override void FixedUpadte()
    {
        //playerController.SetVelocityX(runSpeed); //直接跑动
         playerController.Move(currentSpeed); 
    }

    
}
