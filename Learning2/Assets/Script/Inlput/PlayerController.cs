using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    private PhysicsCheck physicsCheck;
    private PlayerInput playerInput;
    private Rigidbody2D rb2D;
    private Health health;
    public IInteractable currentInteractable;  //可交互物体


    #region 移动
    public bool IsGround => physicsCheck.isGround;
    public bool IsFall => physicsCheck.IsFall;
    public bool IsWall => physicsCheck.isWall;
    public bool CanAirJump { get; set; } = true;
    public float MoveSpeed
    {
        get
        {
            if(rb2D == null) return 0f;
            return Mathf.Abs(rb2D.velocity.x);
        }
    } 
    public float CurrentGravityScale
    {
        get
        {
            if(rb2D == null) return 0f;
            return rb2D.gravityScale;
        }
    }
    
    public float PlayerDirection
    {
        get => transform.localScale.x;
        set
        {
            Vector3 scale = transform.localScale;
            scale.x = value;
            transform.localScale = scale;
        }
    }
    #endregion

    #region 攻击
    private float attackTime;
    public float AttackTime
    {
        get => attackTime;
        set
        {
            attackTime = Mathf.Max(0f, value);
        }
    }
    public bool IsAttackTimerRunning => AttackTime > 0f;
    public int AttackCombo {get;set;}  = 1;
    #endregion

    public bool IsHurt {get;set;}  = false;
    public bool IsDie {get;set;}  = false;



    void Awake()
    {
        physicsCheck = GetComponentInChildren<PhysicsCheck>();
        playerInput = GetComponent<PlayerInput>();
        rb2D = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    void OnEnable()
    {
        if (playerInput != null)
        {
            // 延迟一帧执行，确保 PlayerInput.Awake 已完成
            Invoke(nameof(EnableInputDelayed), 0.01f);
            playerInput.EnableGameplayerInput();
        }
        
        if(health != null)
        {
            health.OnHurt += IsOnHurt;
            health.OnDie += IsOnDie;
        }
    }

    private void EnableInputDelayed()
    {
        if (playerInput != null)
        {
            playerInput.EnableGameplayerInput();
        }
    }

    void OnDisable()
    {
        if(playerInput != null)
        {
            playerInput.DisableGamePlayerInput();
        }
        
        if(health != null)
        {
            health.OnHurt -= IsOnHurt;
            health.OnDie -= IsOnDie;
        }
    }

    void Update()
    {
        AttackTimeCount();
    }


    #region 移动方法
    /// <summary>
    /// 根据玩家输入方向改变移动方向和朝向
    /// </summary>
    /// <param name="speed"></param>
    public void Move(float speed)
    {
        if (playerInput.IsMove)
        {
            transform.localScale = new Vector3(playerInput.AxisX, 1f, 1f);
        }
        SetVelocityX(speed * playerInput.AxisX);
    }
    /// <summary>
    /// 对刚体的速度进行直接修改
    /// </summary>
    /// <param name="velocity"></param>
    public void SetVelocity(Vector2 velocity)
    {
        rb2D.velocity = velocity;
    }
    public void SetVelocity(float v1, float v2)
    {
        rb2D.velocity = new Vector2(v1, v2);
    }
    /// <summary>
    /// 将刚体X轴的速度设为参数的值
    /// </summary>
    /// <param name="velocityX"></param>
    public void SetVelocityX(float velocityX)
    {
        rb2D.velocity = new Vector2(velocityX, rb2D.velocity.y);
    }
    /// <summary>
    /// 将刚体Y轴的速度设为参数的值
    /// </summary>
    /// <param name="velocityY"></param>

    public void SetVelocityY(float velocityY)
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, velocityY);
        // rb2D.AddForce(Vector2.up * velocityY);
    }

    public void SetGravity(float value)
    {
        rb2D.gravityScale = value;
    }
    #endregion

    /// <summary>
    /// 攻击连击时间计时
    /// </summary>
    public void AttackTimeCount()
    {
        if (IsAttackTimerRunning)
        {
            AttackTime -= Time.deltaTime;
            if (AttackTime <= 0f)
            {
                AttackCombo = 1;

            }
        }
    }

    /// <summary>
    /// 重置攻击时间
    /// </summary>
    public void StartAttackTimer(float time)
    {
        attackTime = time;
    }

    public void IsOnHurt()
    {
        IsHurt = true;
       // Debug.Log("Hurt is"+IsHurt);
    }

    public void IsOnDie()
    {
        IsDie = true;
        //Debug.Log("Die is" +IsDie);   
    }

/// <summary>
/// 记录当前可交互物体
/// </summary>
/// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable; //记录当前可交互物体
        }
    }

    ///清除当前可交互物体
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable) && interactable == currentInteractable)
        {
            currentInteractable = null ;
        }
    }


}
