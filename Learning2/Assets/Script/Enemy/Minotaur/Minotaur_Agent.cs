using System;
using Unity.VisualScripting;
using UnityEngine;

public class Minotaur_Agent : MonoBehaviour
{
    #region 组件
    private Rigidbody2D rb2D;
    private CheckPlayer checkPlayer;
    private CheckAttackRange checkAttackRange;
    private EnemyHealth health;

    #endregion

 #region 状态变量
    public bool IsChase {get ; private set;} = false; //是否追击
    public bool IsAttack {get ; private set;} = false; //是否攻击
    public bool IsDied {get ; private set;} = false;
    public bool IsHurt {get ; private set;} = false;
    public bool IsDestory{get ; private set;} = false; //是否可以删除物体
    
    private Transform attackerTransform; //攻击者的位置
    private float knockbackForce; //击退力度
    
    //[SerializeField]public float attackCooldown = 1f; //攻击冷却时间
    private Vector2 playerPosition;//玩家位置
    public readonly Vector2 INVALID_VECTOR2 = Vector2.negativeInfinity;

    [SerializeField] private Transform[] wayPoint;  //巡逻点位
    public Vector2[] WayPointPositions { get ; private set;}
    private int currentPatrolIndex = 0; //当前巡逻点位
    private Vector2 targetWayPoint;  //下一个巡逻点位
    private Vector2 partolDirection;  //巡逻面朝方向
    private Vector2 chaseDirection; //追击面朝方向
    private bool isTimeCount = false; //是否计时
    private float startTime; //状态开始时间
    private bool isAttackColldownTimeCount = false; //是否攻击冷却计时
    private float AttackColldownStartTime ; //攻击冷却开始时间
    

    [Header("站立状态")]
    [SerializeField] private float midIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 4f;
    private float idleTime;  //站立时间
    
    
    
#endregion
    void Awake()
    {
        checkPlayer = GetComponentInChildren<CheckPlayer>();
        checkAttackRange = GetComponentInChildren<CheckAttackRange>();
        rb2D = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();

        InitWayPoints();
    }

    void OnEnable()
    {
        if(checkPlayer != null)
        {
            checkPlayer.IsSeePlayer += OnChase;
            checkPlayer.SetPlayerPosition += OnSetPlayerPosition;
        }

        if(checkAttackRange != null)
        {
            checkAttackRange.IsCanAttack += OnAttack;
        }

        if(health != null)
        {
            health.OnHurt += OnHurt;
            health.OnDie += OnDied;
        }
    }

    void OnDisable()
    {
        if(checkPlayer != null)
        {
            checkPlayer.IsSeePlayer -= OnChase;
            checkPlayer.SetPlayerPosition -= OnSetPlayerPosition;
        }

        if(checkAttackRange != null)
        {
            checkAttackRange.IsCanAttack -= OnAttack;
        }

        if(health != null)
        {
            health.OnHurt -= OnHurt;
            health.OnDie -= OnDied;
        }
    }

    
#region 事件接收
    private void OnAttack(bool canAttack)
    {
        IsAttack = canAttack;
        Debug.Log("可以攻击");
    }

    private void OnSetPlayerPosition(Vector2 vector)
    {
        playerPosition = vector;
        Debug.Log($"获得玩家的位置，当前玩家位置为{playerPosition}");
    }

    private void OnChase(bool canChase)
    {
        IsChase = canChase;
        Debug.Log("可以追击");
    }

    private void OnDied()
    {
        IsDied = true;
        Debug.Log("可以死亡");
    }

    private void OnHurt(Transform transform, float force)
    {
        IsHurt = true;
        attackerTransform = transform;
        knockbackForce = force;
        Debug.Log($"受伤，攻击者位置{attackerTransform}， 击退力度{knockbackForce}");
    }

#endregion

#region 状态重置
/// <summary>
/// 停止追击
/// </summary>
    public void StopChase()
    {
        IsChase = false;    
    }
/// <summary>
/// 攻击停止
/// </summary>
    public void StopAttack()
    {
        IsAttack = false;
    }
/// <summary>
/// 停止受伤状态
/// </summary>
    public void StopHurt()
    {
        IsHurt= false;
    }

/// <summary>
/// 开始计时
/// </summary>
    public void StartTimeCount()
    {
        isTimeCount = true;
    }

    /// <summary>
    /// 可以删除
    /// </summary>
    public void StopStartDestory()
    {
        IsDestory = false;
    }




#endregion
    /// <summary>
    /// 巡逻点位初始化
    /// </summary>
    private void InitWayPoints()
    {
        if(wayPoint == null)
        {
            Debug.LogError("巡逻点位不存在");
        }   
        WayPointPositions = new Vector2[wayPoint.Length];

        for (int i = 0; i < wayPoint.Length; i++)
        {
            WayPointPositions[i] = wayPoint[i].position;
        }
    }
/// <summary>
/// 受伤击退
/// </summary>
    public void Knockback()
    {
        if(attackerTransform == null)
        {
            Debug.Log($"攻击者的transforme 为空 ");
            return;
        }
        Vector2 direction = this.transform.position - attackerTransform.position;

        direction.y = 0;
        if(direction.sqrMagnitude < 0.01f) //该向量的平方长度,高效判断向量是否为零向量
        {
            direction = transform.right;
        }
        else
        {
            direction.Normalize();
        }
        rb2D.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
    #region 巡逻
/// <summary>
/// 巡逻
/// </summary>
    public void Patrol()
    {
        //bool arrvied = Vector2.Distance(transform.position, targetWayPoint) < 0.5f; //当改成0.01f时，update和fixedUpdate不同步，导致距离无法达到
        //bool arrvied = Mathf.Abs(transform.position.x - targetWayPoint.x) < 0.5f;
        Debug.Log($"当前距离为：{Vector2.Distance(transform.position, targetWayPoint)}");
        if (ArrayWayPoint()) //更新CurrentIndex;
        {
            currentPatrolIndex = (currentPatrolIndex +1 ) % WayPointPositions.Length;

            UpdateTargetWayPoint();     
        }
        PartolFaceDirection();
    }

/// <summary>
/// 移动
/// </summary>
/// <param name="speed"></param>
    public void PartolMove(float speed)
    {
        if(partolDirection.x == 0)
        {
            Debug.Log($"当前direction.x值为0");
        }

        if(speed == 0)
        {
            Debug.Log($"当前传入的速度为值为0");
        }
        rb2D.velocity = new Vector2(partolDirection.x  * speed, rb2D.velocity.y);
        Debug.Log($"当前速度为：{rb2D.velocity.x} , {rb2D.velocity.y}");
        
        
    }

/// <summary>
/// 巡逻面朝方向
/// </summary>
    public void PartolFaceDirection()
    {
        partolDirection = (targetWayPoint - (Vector2)this.transform.position).normalized; //与巡逻点之间方向判断
        //Debug.Log($"当前方向为：{direction}");
        
        if(partolDirection.x > 0.01f)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if(partolDirection.x< -0.01f )
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        } 
    }

    

/// <summary>
/// 更新巡逻点位
/// </summary>
    public void UpdateTargetWayPoint()
    {
        if (WayPointPositions == null || WayPointPositions.Length == 0)
        {
            Debug.LogError("巡逻点位为空");
            return;
        }
        
        targetWayPoint = WayPointPositions[currentPatrolIndex];
        Debug.Log($"下一个点位为：{currentPatrolIndex}，坐标{targetWayPoint}");
    } 
/// <summary>
/// 是否达到巡逻点位
/// </summary>
/// <returns></returns>
    public bool ArrayWayPoint()
    {
        if(Mathf.Abs(transform.position.x - targetWayPoint.x) < 0.5f)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region 移动相关

/// <summary>
/// 停止移动
/// </summary>
    public void StopMove()
    {
        rb2D.velocity = Vector2.zero;
    }
    #endregion
    #region 站立状态
    public void SetIdleTime()
    {
        idleTime = UnityEngine.Random.Range(midIdleTime, maxIdleTime);
        startTime = Time.time;
        isTimeCount = true;
        Debug.Log($"当前站立时间为{idleTime},开始时间为{startTime},是否开始计时{isTimeCount}");
    }
/// <summary>
/// 是否结束站立
/// </summary>
/// <returns></returns>
    public bool IsIdleTimerExpired()
    {
        if(!isTimeCount) return false;

        if(Time.time - startTime >= idleTime )
        {
            isTimeCount = false;
            return true;
        }

        return false;
    }

    #endregion

    #region 追击
    /// <summary>
    /// 是否达到玩家附近
    /// </summary>
    /// <returns></returns>
    public bool ArrayPlayerPoint()
    {
        if(Vector2.Distance(this.transform.position, playerPosition) < 0.5f)
        {
            return true;
        }

        return false;
    }
/// <summary>
/// 移动
/// </summary>
/// <param name="speed"></param>
    public void ChaseMove(float speed)
    {
        if(chaseDirection.x == 0)
        {
            Debug.Log($"当前direction.x值为0");
        }

        if(speed == 0)
        {
            Debug.Log($"当前传入的速度为值为0");
        }
        rb2D.velocity = new Vector2(chaseDirection.x  * speed, rb2D.velocity.y);
        Debug.Log($"当前速度为：{rb2D.velocity.x} , {rb2D.velocity.y}");
        
        
    }
/// <summary>
/// 追击时面朝方向
/// </summary>
    public void ChaseFaceDirection()
    {
        chaseDirection = (playerPosition - (Vector2)this.transform.position).normalized; //与巡逻点之间方向判断
        //Debug.Log($"当前方向为：{direction}");
        
        if(chaseDirection.x > 0.01f)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if(chaseDirection.x< -0.01f )
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        } 
    }
    #endregion

    #region 攻击
    /// <summary>
    /// 设置攻击冷却开始时间
    /// </summary>
    public void SetAttackCoolDownTime()
    {
        AttackColldownStartTime = Time.time;
        isAttackColldownTimeCount = true;
    }
/// <summary>
/// 攻击冷却是否结束
/// </summary>
/// <param name="attackCooldown"></param>
/// <returns></returns>
    public bool IsAttackCoolDownExpired(float attackCooldown)
    {
        if(!isAttackColldownTimeCount) return false;

        if(Time.time - AttackColldownStartTime >= attackCooldown)
        {
            isAttackColldownTimeCount = false;
            return true;
        }

        return false;
    }
    #endregion

    #region 死亡状态
/// <summary>
/// 禁用物理模拟
/// </summary>
    public void StopRigidbody()
    {
        rb2D.isKinematic = true; // 关闭物理模拟
        transform.GetComponent<Collider2D>().enabled = false; // 禁用碰撞 
    }
/// <summary>
/// 死亡删除物体
/// </summary>
    public void DestroyObject()
    {
        IsDestory = true;
        Destroy(this.gameObject , 0.2f);  
    }

    #endregion
    
}
