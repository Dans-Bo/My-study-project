using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/AirJump", fileName = "PlayerState_AirJump")]
public class PlayerState_AirJump : PlayerState
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 7f;
   // [SerializeField] ParticleSystem jumpVFX;
    public override void Enter()
    {
        base.Enter();
        playerController.CanAirJump = false;
        playerController.SetVelocityY(jumpForce);
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_PlayerJump2);

        //Instantiate(original: jumpVFX, position: playerController.transform.position, rotation: Quaternion.identity);
    }

    public override void Update()
    {
        if (playerInput.IsStopJump || playerController.IsFall)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Fall));
        }
        if (playerController.IsWall)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_WallGrab));
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
