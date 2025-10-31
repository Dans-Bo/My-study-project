using UnityEngine;
using BehaviourTrees;

public class Minotaur_Chase : Node
{

    [SerializeField] float moveSpeed = 6;

     protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
    {

        var playerPos = blackboard.Get<Vector2>("playerPosition");
        var rb = blackboard.Get<Rigidbody2D>("rb");
        var animator = blackboard.Get<Animator>("animator");
        var isAttacking = blackboard.Get<bool>("isAttacking");

        var distanceToPlayer = Vector2.Distance(transform.position, playerPos);

       // transform.position = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime * moveSpeed);

        Vector2 direction = (playerPos - (Vector2)transform.position).normalized;

       
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
        
        if (Mathf.Abs(distanceToPlayer) < 2f)
        {
            rb.velocity = Vector2.zero;
            return Status.Success;
        } 
        
        return Status.Running;   
    }

}
