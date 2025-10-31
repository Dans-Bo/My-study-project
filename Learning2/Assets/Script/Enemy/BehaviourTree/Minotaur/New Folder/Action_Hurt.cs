using UnityEngine;

namespace BehaviourTrees
{
    public class Action_Hurt : Node
    {
        private float hurtDuration = 0.7f;
        private float timer = 0f;
        private bool started = false;

        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            if (!started)
            {
                started = true;
                timer = hurtDuration;

                var animator = blackboard.Get<Animator>("animator");
                animator?.SetTrigger("Hurt");
            }

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                blackboard.Set("isHurt", false);
                started = false;
                return Status.Success;
            }

            return Status.Running;
        }

        public override void Reset()
        {
            base.Reset();
            started = false;
            timer = 0f;
        }
    }
}