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

        protected override NodeStatus OnEvaluate(Transform transform , Blackboard blackboard)
        {

           while (currentIndex < children.Count)
            {
               NodeStatus status = children[currentIndex].Evaluate(transform, blackboard);
                
                 switch (status)
                {
                    case NodeStatus.Running:
                        return status;
                    case NodeStatus.Failure:
                        Reset();
                        return status;
                    case NodeStatus.Success:
                        currentIndex ++;
                        continue;
                } 
 
            }
            Reset();
            return NodeStatus.Success;
        }
            
        public override void Reset() 
        {
            currentIndex = 0;
            base.Reset(); 
        }  
    }
}

