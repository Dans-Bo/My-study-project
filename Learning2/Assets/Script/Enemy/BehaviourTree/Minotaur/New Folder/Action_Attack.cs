using UnityEngine;

namespace BehaviourTrees
{
    public class Action_Attack : Node
    {
        private float attackCooldown = 1.5f;
        private float cooldownTimer = 0f;

        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            Transform player = blackboard.Get<Transform>("playerTransform");
            if (player == null)
                return Status.Failure;

            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                var animator = blackboard.Get<Animator>("animator");
                animator?.SetTrigger("Attack");
                cooldownTimer = attackCooldown;
            }

            return Status.Running;
        }

        public override void Reset()
        {
            base.Reset();
            cooldownTimer = 0f;
        }
    }
}