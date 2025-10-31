using BehaviourTrees;
using UnityEngine;

public class Minotaur_PatrolTask : Node
{
    private int currentIndex;
    private Vector2[] wayPoint;



    public Minotaur_PatrolTask(Transform[] wayPoints)
    {
        wayPoint = new Vector2[wayPoints.Length];


        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoint[i] = wayPoints[i].position;
        }
        currentIndex = 0;
    }
    
    protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
    {
        
        var moveSpeed = blackboard.Get<float>("moveSpeed");
        var animator = blackboard.Get<Animator>("animator");
        var rb = blackboard.Get<Rigidbody2D>("rb");
        var isAttacking = blackboard.Get<bool>("isAttacking");
        
        bool seePlayer = blackboard.Get<bool>("seePlayer");
        if(seePlayer ) return Status.Failure;
        
        #region 在两个点之间巡逻
        var currentWayPoint = wayPoint[currentIndex];

        Vector2 direction = (currentWayPoint - (Vector2)transform.position).normalized;
        
        bool arrvied = Vector2.Distance(transform.position, currentWayPoint) < 0.1f;

        if (arrvied) //更新CurrentIndex;
        {
            ++currentIndex;
            currentIndex %= wayPoint.Length;
        }

        //移动
        //翻转
        if (direction.x > 0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(0.5f, 0.5f, .5f);
        }  
        if (direction.x < 0)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }

        if(!isAttacking) animator.Play("Walk_Mino");
        return Status.Running;

        #endregion
    }
    

}
