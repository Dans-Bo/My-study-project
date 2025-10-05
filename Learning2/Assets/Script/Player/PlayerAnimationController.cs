using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
/* 
    private Animator anim;
    private Player player;
    private PhysicsCheck pCheck;


    private int speedID;
    private int jumpID;
    private int isGroundID;

    private int isAttackID;
    private int attackID;
    private int hurtID;

    //private int isDownAttackID;
    //private int downAttackID;

    //private int jumpID;
    void Start()
    {
        anim = GetComponent<Animator>();
       // playerController = GetComponent<PlayerController>();
        pCheck = GetComponent<PhysicsCheck>();

        speedID = Animator.StringToHash("xVelocity");
        jumpID = Animator.StringToHash("yVelocity");
        isGroundID = Animator.StringToHash("isGround");
        isAttackID = Animator.StringToHash("isAttack");
        attackID = Animator.StringToHash("attack");
        hurtID = Animator.StringToHash("hurt");
        //isDownAttackID = Animator.StringToHash("isDownAttack");
        //downAttackID = Animator.StringToHash("DownAttack");
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        anim.SetFloat(speedID, Mathf.Abs(player.rigid2D.velocity.x));
        anim.SetFloat(jumpID, player.rigid2D.velocity.y);
        anim.SetBool(isGroundID, pCheck.isGround);
        //anim.SetBool(isAttackID, player.isAttack);
         //anim.SetBool(isDownAttackID, pMove.isDownAttack);

    }

    public void PlayerAttackAnim()
    {
        anim.SetTrigger(attackID);
    }

    /* public void PlayerDownAttack()
   {
       anim.SetTrigger(downAttackID);
   }  

    public void PlayerHurt()
    {
        anim.SetTrigger(hurtID);
    }
 */
}
