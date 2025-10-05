using UnityEngine;

public class PlayerState : ScriptableObject,IState
{
    #region 动画变量
    [SerializeField] string stateAnimatorName; //状态动画名
    [SerializeField,Range(0f,1f)] float transitionDuration = 0.1f; //状态动画淡入淡出时间
    private int stateAnimatorNameHash; //动画播放哈希值
    protected float stateStarTime; //状态动画开始时间
    protected float StateDuration => Time.time - stateStarTime; //状态动画持续时间
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length; //是否结束动画播放
    
    #endregion
    protected float currentSpeed; //玩家当前移动速度
    
    #region 组件获取
    protected Animator animator;
    protected PlayerStateMachine playerStateMachine;

    protected PlayerInput playerInput;
    protected PlayerController playerController;
    #endregion

    void OnEnable()
    {
        stateAnimatorNameHash = Animator.StringToHash(stateAnimatorName);
    }
    /// <summary>
    /// 初始化组件，提供引用 （依赖注入方法）
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="playerStateMachine"></param>
    /// <param name="playerInput"></param>
    /// <param name="playerController"></param>
    public void Initialize(Animator animator, PlayerStateMachine playerStateMachine, PlayerInput playerInput, PlayerController playerController)
    {
        this.animator = animator;
        this.playerStateMachine = playerStateMachine;
        this.playerInput = playerInput;
        this.playerController = playerController;
    }
    public virtual void Enter()
    {
        animator.CrossFade(stateAnimatorNameHash, transitionDuration);
        stateStarTime = Time.time;
    }

    public virtual void Exit()
    {
        
    }

    public virtual void FixedUpadte()
    {
       
    }

    public virtual void Update()
    {
        //Debug.Log(StateDuration);
    }
}
