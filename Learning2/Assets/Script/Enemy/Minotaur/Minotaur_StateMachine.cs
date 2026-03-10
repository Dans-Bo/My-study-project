using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Minotaur_StateMachine: StateMachine
{
    [SerializeField] Minotaur_State[] states; //创建状态数组
    [SerializeField] private Transform[] wayPoint;  //巡逻点位
    public Vector2[] WayPointPositions { get ; private set;}
    public System.Type CurrentStateType => currentState?.GetType();

    private Animator animator;
    private Rigidbody2D rb;
    private CheckPlayer checkPlayer;
    private CheckAttackRange checkAttackRange;
    private EnemyHealth health;


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        checkPlayer = GetComponentInChildren<CheckPlayer>();
        checkAttackRange = GetComponentInChildren<CheckAttackRange>();
        health = GetComponent<EnemyHealth>();
        stateTable  = new Dictionary<System.Type, IState>(states.Length);
        

        //巡逻点位初始化
        

        if(wayPoint == null)
        {
            Debug.LogError("巡逻点位不存在");
        }
        
        WayPointPositions = new Vector2[wayPoint.Length];

        for (int i = 0; i < wayPoint.Length; i++)
        {
            WayPointPositions[i] = wayPoint[i].position;
        }

        foreach (Minotaur_State state in states) //遍历状态数组里的所有状态，并初始化所需要的参数
        {
            state.Initialize(animator, this, rb, transform,checkAttackRange,checkPlayer,health);
            stateTable.Add(state.GetType(), state); 
        }
    }

    void Start()
    {
        SwitchOn(stateTable[typeof(Minotaur_Idle)]); //初始化为站立状态
        Debug.Log("初始化为站立状态");
    }

    /// <summary>
    /// 编辑器停止运行时，清理所有状态的事件
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStateMachines()
    {
        var allStateMachines = FindObjectsOfType<Minotaur_StateMachine>();
        foreach (var sm in allStateMachines)
        {
            sm.CleanupAllStates();
        }
    }

    /// <summary>
    /// 清理所有状态的事件订阅
    /// </summary>
    private void CleanupAllStates()
    {
        if (stateTable == null) return;
        
        foreach (var state in stateTable.Values)
        {
            if (state is Minotaur_State minotaurState)
            {
                minotaurState.DisableEvents();
            }
        }
        
        // 退出当前状态并解绑
        currentState?.Exit();
        currentState = null;
    }

    /// <summary>
    /// 确保对象销毁时清理事件
    /// </summary>
    protected  void OnDestroy()
    {
        CleanupAllStates();
    }

    public void SwitchMinotaurState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Attack:
                SwitchState(stateTable[typeof(Minotaur_Attack)]);
                //Debug.Log("切换到攻击状态");
                break;
            case EnemyState.Idle:
                SwitchState(stateTable[typeof(Minotaur_Idle)]);
                //Debug.Log("切换到站立状态");
                break;
            case EnemyState.Partol:
                SwitchState(stateTable[typeof(Minotaur_Patrol)]);
                //Debug.Log("切换到巡逻状态");
                break;
            case EnemyState.Chase:
                SwitchState(stateTable[typeof(Minotaur_Chase)]);
                //Debug.Log("切换到追击状态");
                break;
            case EnemyState.Hurt:
                SwitchState(stateTable[typeof(Minotaur_Hurt)]);
                //Debug.Log("切换到受伤状态");
                break;
            case EnemyState.Died:
                SwitchState(stateTable[typeof(Minotaur_Died)]);
                //Debug.Log("切换到死亡状态");
                break;
            case EnemyState.AttackCooldown:
                SwitchState(stateTable[typeof(Minotaur_AttackCooldown)]);;
                //Debug.Log("切换到攻击冷却状态");
                break;
        }
    }

    
}

