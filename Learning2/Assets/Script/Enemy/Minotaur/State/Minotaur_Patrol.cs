
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Partol", fileName = "Partol")]
public class Minotaur_Patrol:Minotaur_State
{
    private int currentPatrolIndex = 0;  //当前巡逻点位
    [SerializeField] float partolSpeed; //巡逻速度
    
    private Vector2 direction;  //面朝方向
    private Vector2 targetWayPoint;  //下一个巡逻点位

    public override void Enter()
    {
        base.Enter();
        UpdateTargetWayPoint();
        
    }
        
    public override void Update()
    {
        base.Update();

        SwitchState();

        //animator.SetFloat("speed" , partolSpeed);

        PatrolMovement();

        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb2D.velocity = new Vector2(direction.x * partolSpeed, rb2D.velocity.y);
        
    }

    public override void Exit()
    {
        base.Exit();
        rb2D.velocity = Vector2.zero;
    }
    /// <summary>
    /// 巡逻
    /// </summary>
    private void PatrolMovement()
    {
        
        FaceDirection();
        
        //bool arrvied = Vector2.Distance(transform.position, targetWayPoint) < 0.5f; //当改成0.01f时，update和fixedUpdate不同步，导致距离无法达到
        bool arrvied = Mathf.Abs(transform.position.x - targetWayPoint.x) < 0.5f;
        Debug.Log($"{Vector2.Distance(transform.position, targetWayPoint)}");
        if (arrvied) //更新CurrentIndex;
        {
            currentPatrolIndex = (currentPatrolIndex +1 ) % stateMachine.WayPointPositions.Length;

            UpdateTargetWayPoint();

            stateMachine.SwitchMinotaurState(EnemyState.Idle);
        }



    }

/// <summary>
/// 朝向
/// </summary>
    private void FaceDirection()
    {
        direction = (targetWayPoint - (Vector2)transform.position).normalized; //与巡逻点之间方向判断
        //Debug.Log($"当前方向为：{direction}");
        
        if(direction.x > 0.01f)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if(direction.x< -0.01f )
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }

    }
/// <summary>
/// 状态转换
/// </summary>
    private void SwitchState()
    {
        if(isAttack)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Attack); 
        }

        if(isChase)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Chase);
        }
    }
    /// <summary>
    /// 更新巡逻点位
    /// </summary>
    private void UpdateTargetWayPoint()
    {
        if (stateMachine.WayPointPositions == null || stateMachine.WayPointPositions.Length == 0)
        {
            Debug.LogError("巡逻点位为空");
            return;
        }
        
        targetWayPoint = stateMachine.WayPointPositions[currentPatrolIndex];
        Debug.Log($"下一个点位为：{currentPatrolIndex}，坐标{targetWayPoint}");
    }

}
