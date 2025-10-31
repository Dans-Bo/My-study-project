using UnityEngine;

namespace BehaviourTrees
{
    public class Action_Idle : Node
    {
        private float idleTime;
        private float timer;
        private bool isInitialized;

        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            if (!isInitialized)
            {
                idleTime = Random.Range(1f, 3f);
                timer = 0f;
                isInitialized = true;

                var animator = blackboard.Get<Animator>("animator");
                animator?.SetTrigger("Idle");
            }

            timer += Time.deltaTime;
            if (timer >= idleTime)
            {
                isInitialized = false;
                return Status.Success;
            }

            return Status.Running;
        }

        public override void Reset()
        {
            base.Reset();
            isInitialized = false;
            timer = 0f;
        }
    }
}