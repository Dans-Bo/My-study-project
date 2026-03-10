using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace BehaviourTrees
{
    public  class Selector : Node
    {
        private int currentIndex = 0;

        public Selector(List<Node> children)
        {
            foreach(var child in children)
            {
                AddChild(child);
            }
        }

        protected override NodeStatus OnEvaluate(Transform transform, Blackboard blackboard)
        {
            //带记忆节点
            while ( currentIndex < children.Count)
            {
                NodeStatus status = children[currentIndex].Evaluate(transform, blackboard);

                switch (status)
                {
                    case NodeStatus.Success:
                        Reset();
                        return status;
                    case NodeStatus.Failure:
                        currentIndex ++ ;
                        continue;
                    case NodeStatus.Running:
                        return status;
                }
            }

            Reset();
            return NodeStatus.Failure;
        }

        public override void Reset()
        {
            currentIndex = 0;
            base.Reset();   
        }   
    }
}
