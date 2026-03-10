using UnityEngine;
using BehaviourTrees;

[CreateAssetMenu(menuName = "Data/StateMachine/Enemy/MinotaurState/Chase", fileName = "Chase")]
public class Minotaur_Chase : Minotaur_State
{
    [SerializeField] private float chaseSpeed;
    private Vector2 direction;

    public override void Update()
    {
        base.Update();

        if (playerPosition == INVALID_VECTOR2)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Idle);
            return;
        }


        FaceDirection();

        bool arrvied = Vector2.Distance(transform.position, playerPosition) < 0.5f;
        
        if(arrvied || !isChase)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Idle);
        }

        if(isAttack)
        {
            stateMachine.SwitchMinotaurState(EnemyState.Attack);
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rb2D.velocity = new Vector2(direction.x * chaseSpeed, rb2D.velocity.y);
    }

    private void FaceDirection()
    {
        direction =  (playerPosition - (Vector2)transform.position).normalized;

        if(direction.x > 0.01f)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if(direction.x<0.01f )
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isChase = false;
        
    }


}
