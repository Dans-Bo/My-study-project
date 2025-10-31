using UnityEngine;

namespace BehaviourTrees
{
    public class Action_Patrol : Node
    {
        private Transform[] patrolPoints;
        private float speed = 2f;
        private bool isIdle = false;
        private Action_Idle idleNode = new Action_Idle();

        public Action_Patrol(Transform[] points)
        {
            patrolPoints = points;
        }

        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            if (isIdle)
            {
                var idleStatus = idleNode.Evaluate(transform, blackboard);
                if (idleStatus == Status.Running)
                    return Status.Running;

                isIdle = false;
                idleNode.Reset();
            }

            if (patrolPoints == null || patrolPoints.Length == 0)
                return Status.Failure;

            int index = blackboard.Get<int>("currentPatrolIndex");
            Transform target = patrolPoints[index];

            Vector2 newPos = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            // 面向方向
            if (target.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            // 到达点
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                if (Random.value < 0.3f)
                {
                    isIdle = true;
                    return Status.Running;
                }

                index = (index + 1) % patrolPoints.Length;
                blackboard.Set("currentPatrolIndex", index);
            }

            return Status.Running;
        }

        public override void Reset()
        {
            base.Reset();
            isIdle = false;
            idleNode.Reset();
        }
    }
}