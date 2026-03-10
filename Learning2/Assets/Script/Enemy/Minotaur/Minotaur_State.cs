using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Minotaur_State : ScriptableObject, IState
{
#region 组件
    protected Animator animator;
    protected Minotaur_StateMachine stateMachine;
    protected Rigidbody2D rb2D;
    protected Transform transform;
    protected CheckPlayer checkPlayer;
    protected CheckAttackRange checkAttackRange;
    protected EnemyHealth health;

#endregion

#region 状态变量
    protected bool isChase = false; //是否追击
    protected bool isAttack = false ; //是否攻击
    protected bool isDied = false ; 
    protected bool isHurt = false ;
    protected Transform attackerTransform; //攻击者的位置
    protected float knockbackForce; //击退力度
    
    //[SerializeField]protected float attackCooldown = 1f; //攻击冷却时间
    protected Vector2 playerPosition; //玩家位置
    protected readonly Vector2 INVALID_VECTOR2 = Vector2.negativeInfinity;  //无效值，在退出追击后，玩家位置值无效
    public bool IsStateCompleted {get; protected set ;}  //状态是否完成

#endregion

#region 动画相关变量

    [SerializeField] string stateAnimatorName; //状态动画名
    //[SerializeField,Range(0f,1f)] float transitionDuration = 0.1f; //状态动画淡入淡出时间
    private int stateAnimatorNameHash; //动画播放哈希值
    protected float stateStarTime; //状态动画开始时间
    protected float StateDuration => Time.time - stateStarTime; //状态动画持续时间
    //protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length; //是否结束动画播放
    protected bool IsAnimationFinished 
    { 
        get 
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(0);
        
        bool isAnimationMatch = currentState.shortNameHash == stateAnimatorNameHash;
        bool isDurationEnough = StateDuration >= currentState.length;

    
        Debug.Log($"【动画完成判断】\n" +
                  $"动画匹配结果：{isAnimationMatch} | 时长达标结果：{isDurationEnough}\n" +
                  $"当前动画哈希：{currentState.shortNameHash} | 目标动画哈希：{stateAnimatorNameHash}\n" +
                  $"状态已运行时长：{StateDuration:F2}s | 动画总长度：{currentState.length:F2}s\n" +
                  $"当前播放动画名：{(animator.GetCurrentAnimatorClipInfo(0).Length > 0 ? animator.GetCurrentAnimatorClipInfo(0)[0].clip.name : "无动画")}");
            
        return isAnimationMatch && isDurationEnough;
            /* var currentState = animator.GetCurrentAnimatorStateInfo(0);
            // 确保当前播放的是目标状态动画，且时长足够
            return currentState.shortNameHash == stateAnimatorNameHash && 
                   StateDuration >= currentState.length; */
        } 
    }
#endregion
    public void Initialize(Animator animator, Minotaur_StateMachine stateMachine , Rigidbody2D rb, Transform transform,
                           CheckAttackRange checkAttackRange , CheckPlayer checkPlayer , EnemyHealth health )
    {
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.rb2D = rb;
        this.transform = transform;
        this.checkPlayer = checkPlayer;
        this.checkAttackRange = checkAttackRange;
        this.health = health;
        
        DisableEvents();
        AddEvents(); 

        Debug.Log($"当前是否受伤:{isHurt} ,");// 攻击者的位置:{attackerTransform.position}
    }

    

    void OnEnable()  //将动画名转换成哈希值，
    {
        stateAnimatorNameHash = Animator.StringToHash(stateAnimatorName);
    }



    void OnDisable()
    {
        DisableEvents();
    }
    #region 事件

    protected void OnDied()
    {
        isDied = true ;
    }
    /// <summary>
    /// 受伤时间，传输攻击者位置和击退力
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="force"></param>
    protected void OnHurt(Transform transform, float force)
    {
        SetHurtState(true,transform,force);
    }
    /// <summary>
    /// 攻击通知
    /// </summary>
    /// <param name="canAttack"></param>
    protected void OnAttack(bool canAttack)
    {
        isAttack = canAttack;
    }
    /// <summary>
    /// 追击通知
    /// </summary>
    /// <param name="canSeePlayer"></param>
    protected void OnChase(bool canSeePlayer)
    {
        isChase = canSeePlayer;
        //Debug.Log($"是否处于追击状态: {isChase}");
    }
    /// <summary>
    /// 追击获得玩家位置
    /// </summary>
    /// <param name="vector"></param>
    protected void OnSetPlayerPosition(Vector2 vector)
    {
        playerPosition = vector;
        //Debug.Log($"当前追击位置: {playerPosition}");
    }


/// <summary>
/// 取消事件监听
/// </summary>
    public void DisableEvents()
    {
        if(checkPlayer != null)
        {
            checkPlayer.IsSeePlayer -= OnChase ;
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
/// <summary>
/// 事件注册
/// </summary>
    private void AddEvents()
    {
        if(checkPlayer != null)
        {    
            checkPlayer.IsSeePlayer += OnChase ;
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
    #endregion
    /// <summary>
    /// 重置完成状态（通知行为树）
    /// </summary>
    protected void ResetCompleted() => IsStateCompleted = false;
    /// <summary>
    /// 标记状态完成
    /// </summary>
    protected void MarkCompleted() => IsStateCompleted = true;
    /// <summary>
    /// 设置受伤状态
    /// </summary>
    /// <param name="isHurtActive"></param>
    /// <param name="attacker"></param>
    /// <param name="force"></param>
    protected void SetHurtState(bool isHurtActive, Transform attacker = null, float force = 0f)
    {
        isHurt = isHurtActive;
        if(isHurtActive)
        {
            attackerTransform = attacker;
            knockbackForce = force;
        }
        else
        {
            attackerTransform = null;
            knockbackForce = 0f;
        }
    }
    /// <summary>
    /// 重置受伤状态
    /// </summary>
    protected void ResetHurtState()
    {
        SetHurtState(false);
    }

    public virtual void Enter()
    {
        //IsStateCompleted = false;  //状态进入重置

        //animator.CrossFade(stateAnimatorNameHash, transitionDuration);  //平滑切换到当前状态的动画
        stateStarTime = Time.time; //记录状态开始时间

    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Update()
    {
        if(isHurt)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Hurt);
        } 
    }

    public virtual void Exit()
    {
        
    }

    void OnDestroy()
    {
        //编辑器停止运行时强制清理事件
        DisableEvents();
    
    }

}