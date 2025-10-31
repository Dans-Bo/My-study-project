using UnityEngine;

namespace BehaviourTrees
{
    public class Action_Chase : Node
    {
        private float speed = 3f;

        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            Transform player = blackboard.Get<Transform>("playerTransform");
            if (player == null)
                return Status.Failure;

            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPos = (Vector2)transform.position + direction * speed * Time.deltaTime;
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            // 翻转方向
            if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            return Status.Running;
        }
    }
}