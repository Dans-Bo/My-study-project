/* using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerOne : MonoBehaviour
{

    

    
    private PlayerInputController playerInputController;
    private PhysicsCheck physicsCheck; //物理检测

    private PlayerAnimationController playerAnima; //攻击（动画）

    [HideInInspector] public Rigidbody2D rigid2D;
    private BoxCollider2D Collider2D;

    //move
    [Header("移动属性")]
    private Vector2 playerDiration;
    public float moveSpeed;


    [Header("预输入时间")]
    public float jumpBufferTime;
    private float jumpBufferCounter; // 缓冲计数器

    public float coyoteTime;
    private float coyouteTimeCounter; //土狼时间计数器
    //jump
    [Header("跳跃属性")]
    public float jumpForce;
    public float jumpHoldForce;
    public float jumpTime;

    //private int jumpNum;

    //attack
    [Header("攻击")]
    public bool isAttack;
    public bool isDownAttack;
    public bool isHurt;
    public float attackMoveSpeed;

    public float isHurtForce;

    //[SerializeField] float jumpHeight = 5;
    [SerializeField] float gravityScale = 1;
    [SerializeField] float fallGravityScale = 5;

    [Header("连击攻击")]

    public float attackEndTime;
    public float attackEndCounter;
    public int attakCombo = 1;
    public bool isAttackState;
    public bool isAttackStateEnd;



    void Awake()
    {

        
        playerInputController = new PlayerInputController();

        playerInputController.Game.Jump.started += PressedJump;
        playerInputController.Game.Attack.started += PlayerAttack;
    }

    private void OnEnable()
    {
        playerInputController.Enable();

       
    }

    private void OnDisable()
    {
        playerInputController.Disable();

    }


       void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<BoxCollider2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnima = GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        playerDiration = playerInputController.Game.Move.ReadValue<Vector2>();//读取input manager 里gameplay的move的vector2的值
        JumpGravityScale();
        
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
        {
            Move();
        }

        Jump();
    }

    #region 移动
    private void Move()
    {

        rigid2D.velocity = new Vector2(moveSpeed * playerDiration.x, rigid2D.velocity.y);
        PlayerFaceDiration();

    }

    void PlayerFaceDiration()
    {
        if (playerDiration.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (playerDiration.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
    #endregion

    #region 跳跃
    private void PressedJump(InputAction.CallbackContext context)
    {
        //jumpTime = Time.deltaTime + 0.2f;
        jumpBufferCounter = jumpBufferTime;
        rigid2D.gravityScale = gravityScale;
        //float jumpSpeed = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * rigid2D.gravityScale) - 2) * rigid2D.mass; // 固定跳跃高度
    }

    private void Jump()
    {
        
        //土狼时间
        if (physicsCheck.isGround)
        {
            coyouteTimeCounter = coyoteTime;
        }
        else
        {
            coyouteTimeCounter -= Time.deltaTime;
        }
        //预输入
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0)
        {
            if (coyouteTimeCounter > 0 || physicsCheck.isGround)
            {
                rigid2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                jumpBufferCounter = 0f;
                coyouteTimeCounter = 0f;
                //Debug.Log("缓冲跳跃");
            }
        }
    }


    //上升下降给不同的重力比例
    void JumpGravityScale()
    {

        if (rigid2D.velocity.y > 0)
            rigid2D.gravityScale = gravityScale;
        if (rigid2D.velocity.y < 0)
            rigid2D.gravityScale = fallGravityScale;
    }
    #endregion

    //攻击
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround && !physicsCheck.isJump)
        {
            
            playerAnima.PlayerAttackAnim();
            rigid2D.velocity = new Vector2(attackMoveSpeed * transform.localScale.x, rigid2D.velocity.y);
            isAttack = true;
        }
      
    }

    public void GetHurt(Transform trans)
    {
        isHurt = true;

        rigid2D.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - trans.position.x, 0).normalized; //计算方向，当前任务坐标减去攻击者的坐标
        rigid2D.AddForce(dir * isHurtForce, ForceMode2D.Impulse);
    }
}
 */