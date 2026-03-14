using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Minotaur_State : ScriptableObject, IState
{
#region 组件
    protected Animator animator;
    protected Minotaur_StateMachine stateMachine;
    protected Minotaur_Agent minotaur_Agent;

#endregion

#region 状态变量
 
    //public bool IsStateCompleted {get; protected set ;}  //状态是否完成

#endregion

#region 动画相关变量
    protected float StateDuration => stateMachine.StateDuration;
    [SerializeField] string stateAnimatorName; //状态动画名
    //[SerializeField,Range(0f,1f)] float transitionDuration = 0.1f; //状态动画淡入淡出时间
    private int stateAnimatorNameHash; //动画播放哈希值
    protected float stateStarTime; //状态动画开始时间

    protected bool IsAnimationFinished 
    { 
        get 
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(0);
        
            bool isAnimationMatch = currentState.shortNameHash == stateAnimatorNameHash;
            bool isDurationEnough = StateDuration >= currentState.length;

        /* Debug.Log($"动画匹配结果：{isAnimationMatch} | 时长达标结果：{isDurationEnough}\n" +
                  $"当前动画哈希：{currentState.shortNameHash} | 目标动画哈希：{stateAnimatorNameHash}\n" +
                  $"状态已运行时长：{StateDuration:F2}s | 动画总长度：{currentState.length:F2}s\n" +
                  $"当前播放动画名：{(animator.GetCurrentAnimatorClipInfo(0).Length > 0 ? animator.GetCurrentAnimatorClipInfo(0)[0].clip.name : "无动画")}");
             */
            return isAnimationMatch && isDurationEnough;
        } 
    }
#endregion
    
    /// <summary>
    /// 初始化相关组件
    /// </summary>
    public void Initialize(Animator animator, Minotaur_Agent agent ,Minotaur_StateMachine stateMachine )
    {
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.minotaur_Agent = agent;

        if (string.IsNullOrEmpty(stateAnimatorName))
        {
            Debug.LogError($"状态SO {name} 的stateAnimatorName为空", this);
        }
        else if (stateAnimatorNameHash == 0)
        {
            stateAnimatorNameHash = Animator.StringToHash(stateAnimatorName);
        }
            
    }


    public virtual void Enter()
    {
        stateStarTime = Time.time; //记录状态开始时间
        /* if (stateAnimatorNameHash != 0) //动画播放
        {
            animator.CrossFade(stateAnimatorNameHash, 0.1f);
        } */
            
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        
    }

}