using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace BehaviourTrees
{
    public class Sequencer : Node
    {
        private int currentIndex = 0;
        public Sequencer(List<Node> children)
        {
           foreach(var child in children)
            {
                AddChild(child);
            }
        }

        protected override Status OnEvaluate(Transform transform , Blackboard blackboard)
        {

           for (; currentIndex < children.Count; ++currentIndex)
            {
               Status status = children[currentIndex].Evaluate(transform, blackboard);
                
                 switch (status)
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        currentIndex = 0;
                        return Status.Failure;
                    case Status.Success:
                        continue;
                } 
 
            }
            currentIndex = 0;
            return Status.Success;
        }
            
        public override void Reset() 
        {
            base.Reset();
            currentIndex = 0;
        }  
    }
}

