using UnityEngine;

public class Minotaur : Enemy
{

    public override void Move()
    {
        base.Move();
        animator.SetFloat("speed",Mathf.Abs(rb.velocity.x));
        
    }


}
