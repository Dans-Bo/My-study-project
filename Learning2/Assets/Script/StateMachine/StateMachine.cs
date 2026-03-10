using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 管理所有的状态和实现状态机更新
/// </summary>
public class StateMachine : MonoBehaviour
{
    protected IState currentState;

    protected Dictionary<System.Type, IState> stateTable;

    protected virtual void Update()
    {
        currentState.Update();
    }

    protected virtual void  FixedUpdate()
    {
        currentState.FixedUpdate();
    }
    /// <summary>
    /// 状态启动
    /// </summary>
    /// <param name="newState"></param>
    protected void SwitchOn(IState newState)
    {
        currentState = newState;
        currentState.Enter();
    }
    /// <summary>
    /// 状态切换
    /// </summary>
    public void SwitchState(IState newState)
    {
        if (currentState == newState) return;
        
        if(currentState != null)
        {
            currentState.Exit();
        }

        SwitchOn(newState);
    }
    public void SwitchState(System.Type newStateType)
    {
        SwitchState(stateTable[newStateType]);
    }

}
