
using System.Collections.Generic;

using UnityEngine;


public enum PlayerStateType
{
    Idle,Move,Jump,Attack
}
public class Player : MonoBehaviour
{
    [Header("获取玩家输入")]
    [SerializeField] private PlayerInputone playerinputone;

    [Header("移动属性")]
     
    [SerializeField] private float moveSpeed = 5f;
    private  Vector2 playerDiration;
    [Header("跳跃属性")]
    [SerializeField] private float jumpForce = 10f;
    public float gravityScale = 1f;
    public float fallGravityScale = 5f;
    public float jumpBufferTime = 0.2f;
    public float jumpBufferTimeCount;
    public float coyouteTime = 0.1f;
    public float coyouteTimeCounter;
    public bool pressedJump;


    #region 获取组件
    [SerializeField] private Animator playerAnima;
    public Rigidbody2D rigid2D;
    [SerializeField] private PhysicsCheck physicsCheck; //物理检测
    /* [SerializeField] private JumpHandler jumpHandler;
    [SerializeField] private InputBufferSyetem inputBufferSyetem; */
    
    #endregion
    private BaseState currentState; //当前状态
    //private PlayerStateType currentStateType; // 当前状态类型

    //字典Dictionary<键，值>对
    private Dictionary<PlayerStateType, BaseState> states = new Dictionary<PlayerStateType, BaseState>();//存储玩家状态的字典

    //提供只读属性，可被外部访问
    public Vector2 PlayerDirection => playerDiration;
    //public Rigidbody2D Rigidbody2D => rigid2D;
    public Animator Animator => playerAnima;
    public PhysicsCheck PhysicsCheck => physicsCheck;
    //public PlayerStateType CurrentStateType => currentStateType;

   // public JumpHandler JumpHandler => jumpHandler;
    //public InputBufferSyetem InputBufferSyetem => inputBufferSyetem;

    public float MoveSpeed => moveSpeed;
    public float JumpForce => jumpForce;
    
    void Awake()
    {
        if (rigid2D == null) rigid2D = GetComponent<Rigidbody2D>();
        if (playerAnima == null) playerAnima = GetComponent<Animator>();
        if (physicsCheck == null) physicsCheck = GetComponent<PhysicsCheck>();
/* 
        jumpHandler = gameObject.AddComponent<JumpHandler>();
        jumpHandler.Initialize(rigid2D,physicsCheck);

        inputBufferSyetem =new InputBufferSyetem();
        inputBufferSyetem.RegisterBuffer("Jump", 0.2f);
 */
        InitializeStateMachine();

        if (playerinputone != null)
        {
             playerinputone.EnableGameInput();
        }
       
    }

    private void OnEnable()
    {
        if (playerinputone != null)
        {
            playerinputone.Move += OnMoveInput;
            playerinputone.Attack += OnAttackInput;
            playerinputone.Jump += OnJumpInput;
        }
    }

    private void OnDisable()
    {
        if (playerinputone != null)
        {
            playerinputone.Move -= OnMoveInput;
            playerinputone.Attack -= OnAttackInput;
            playerinputone.Jump -= OnJumpInput;
        }
    }
    
    private void Update()
    {
        // jumpHandler.Update();
        // inputBufferSyetem.Update();
        if (PhysicsCheck.isGround)
        {
            coyouteTimeCounter = coyouteTime;
        }
        else
        {
            coyouteTimeCounter -= Time.deltaTime;
        }
        if (jumpBufferTimeCount > 0)
        {
            jumpBufferTimeCount -= Time.deltaTime;
        }
        currentState?.OnUpdate();
        //CheckStateTransitions();
    }

    private void FixedUpdate()
    {
        currentState?.OnFixedUpdate();  
    }

    /// <summary>
    /// 将状态添加到字典里
    /// </summary>
    private void InitializeStateMachine()
    {
        states.Add(PlayerStateType.Idle, new PlayerIdleState(this));
        states.Add(PlayerStateType.Move, new PlayerMoveState(this));
        states.Add(PlayerStateType.Jump, new PlayerJumpState(this));
        states.Add(PlayerStateType.Attack, new PlayerAttackState(this));

        TransitionState(PlayerStateType.Idle);
    }

    /// <summary>
    /// 玩家状态切换
    /// </summary>
    /// <param name="Type"></param>
    public void TransitionState(PlayerStateType Type)
    {
        //如果当前状态不为空，则退出当前状态
        if (currentState != null)
        {
            currentState.OnExit();
        }
        //通过字典的键来找到对应的状态,进入新状态
        currentState = states[Type];
        //currentStateType = Type;
        currentState.OnEnter();
        Debug.Log(currentState);

    }


    /*     
     /// <summary>
      /// 检测状态转换
      /// </summary>
        private void CheckStateTransitions()
        {
            foreach (PlayerStateType stateType in Enum.GetValues(typeof(PlayerStateType)))
            {
                //跳过当前状态
                if (stateType == currentStateType) continue;

                //检查能否度过这个状态
                if (states[stateType].CanTransitionTo(stateType))
                {
                    TransitionState(stateType);
                    break; // 一次只转换一个状态
                }
            }
        } */

    // 在Update中调用
   
    public void OnJumpInput()
    {
        jumpBufferTimeCount =jumpBufferTime;
        pressedJump = true;

        //Debug.Log("jump");
    }
   


    private void OnAttackInput()
    {
   
        Debug.Log("Attack");
    }
    public void OnMoveInput(Vector2 moveInput)
    {
        playerDiration = moveInput;

    }
    public void Move()
    {
        rigid2D.velocity = new Vector2(moveSpeed * playerDiration.x, rigid2D.velocity.y);

        if (playerDiration.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (playerDiration.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
    
    
}
