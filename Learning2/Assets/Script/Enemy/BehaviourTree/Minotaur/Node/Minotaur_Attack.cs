using System.Collections;
using BehaviourTrees;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Minotaur_Attack : Node
{
    bool isAttacking = false;
    float cooldownTime = 0f;
    protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
    {
        var animator = blackboard.Get<Animator>("animator");
        var playerPos = blackboard.Get<Vector2>("playerPosition");
        var rb = blackboard.Get<Rigidbody2D>("rb");
        var attackRange = blackboard.Get<float>("attackRange");
        var attackColdown = blackboard.Get<float>("attackColdown");

        var distance = Vector2.Distance(transform.position, playerPos);

        if (distance > attackRange) //玩家超出攻击距离时，返回失败
        {
            isAttacking = false;
            blackboard.Set("isAttacking", false);
            return Status.Failure;
        }

        if (cooldownTime > 0f) //如果在冷却时间，返回运行中
        {
            Debug.Log("攻击冷却");
            cooldownTime -= Time.deltaTime;
            return Status.Running;
        }

        if (!isAttacking) //如果不在攻击中
        {
            Debug.Log("攻击");
            isAttacking = true;
            blackboard.Set("isAttacking", true);

            animator.Play("Attack_Mino");

            rb.velocity = Vector2.zero;
            cooldownTime = attackColdown;

            transform.GetComponent<MonoBehaviour>().StartCoroutine(AttackRoutine(blackboard));
        }

        return Status.Failure;
    }

    IEnumerator AttackRoutine(Blackboard blackboard)
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        blackboard.Set("isAttacking", false);
    }
        
}
