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

        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {

            //带记忆节点
            for (; currentIndex < children.Count; ++currentIndex)
            {
                Status status = children[currentIndex].Evaluate(transform, blackboard);

                switch (status)
                {
                    case Status.Success:
                        currentIndex = 0;
                        status = Status.Success;
                        return status;
                    case Status.Failure:
                        continue;
                    case Status.Running:
                        status = Status.Running;
                        return status;
                }
            }

            currentIndex = 0;
            return Status.Failure;
        }

        public override void Reset()
        {
            base.Reset();
            currentIndex = 0;
        }   
    }
}
