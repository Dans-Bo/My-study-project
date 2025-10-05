
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerStateMachine : StateMachine
{
    [SerializeField] PlayerState[] states; //创建状态数组

    Animator animator;

    PlayerInput playerInput;
    PlayerController playerController;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();

        stateTable = new Dictionary<System.Type, IState>(states.Length);


        foreach (PlayerState state in states) //遍历状态数组里的所有状态，并初始化所需要的参数
        {
            state.Initialize(animator, this, playerInput, playerController);
            stateTable.Add(state.GetType(), state);
        }
    }

    void Start()
    {
        SwitchOn(stateTable[typeof(PlayerState_Idle)]); //初始化为站立状态
    }
    
 
}

