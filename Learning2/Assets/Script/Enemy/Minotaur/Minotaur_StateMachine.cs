using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Minotaur_StateMachine: StateMachine
{
    [SerializeField] Minotaur_State[] states; //创建状态数组


    private Animator animator;
    private Minotaur_Agent minotaurAgent;


    protected override void Awake()
    {
        base.Awake();
        
        animator = GetComponent<Animator>();
        minotaurAgent = GetComponent<Minotaur_Agent>();

        if(animator == null )
        {
            Debug.Log("animator组件丢失");
        }

        if(minotaurAgent == null)
        {
            Debug.Log("minotaurAgent组件丢失");
        }
        
        foreach (Minotaur_State state in states) //遍历状态数组里的所有状态，并初始化所需要的参数
        {
            state.Initialize(animator,minotaurAgent,this);
            stateTable.Add(state.GetType(), state);
        }
    }

    void Start()
    {
        SwitchOn(stateTable[typeof(Minotaur_Idle)]); //初始化为站立状态
        Debug.Log("初始化为站立状态");
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
