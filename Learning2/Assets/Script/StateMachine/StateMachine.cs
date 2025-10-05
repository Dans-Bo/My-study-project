using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 管理所有的状态和实现状态机更新
/// </summary>
public class StateMachine : MonoBehaviour
{
    IState currentState;

    protected Dictionary<System.Type, IState> stateTable;

    void Update()
    {
        currentState.Update();
    }

    void FixedUpdate()
    {
        currentState.FixedUpadte();
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
        currentState.Exit();
        SwitchOn(newState);
    }
    public void SwitchState(System.Type newStateType)
    {
        
        SwitchState(stateTable[newStateType]);
    }

    /*
    public void SwitchState(IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }
    */
}
