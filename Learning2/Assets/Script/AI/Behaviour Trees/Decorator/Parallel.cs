using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public  class Parallel : Node
    {
        private readonly int successThreshold;
        private readonly int failureThreshold;

        public Parallel(List<Node> children, int successThreshold, int failureThreshold)
        {
            foreach (Node child in children)
            {
                AddChild(child);
            }

            this.successThreshold = successThreshold;
            this.failureThreshold = failureThreshold;
        }
        
        protected override NodeStatus OnEvaluate(Transform transform, Blackboard blackboard)
        {
            int successCount = 0;
            int failureCount = 0;

            foreach (var child in children)
            {
                var status = child.Evaluate(transform, blackboard);

                switch (status)
                {
                    case NodeStatus.Success:
                        successCount++;
                        break;
                    case NodeStatus.Failure:
                        failureCount++;
                        break;
                }

                if (successCount >= successThreshold) return NodeStatus.Success;
                if (failureCount >= failureThreshold) return NodeStatus.Failure;
            }

            return NodeStatus.Running;
        }
    }
}
