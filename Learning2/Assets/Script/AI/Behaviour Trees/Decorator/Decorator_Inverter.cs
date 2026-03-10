using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public class Decorator_Inverter : Node
    {
        public Decorator_Inverter(Node child)
        {
            AddChild(child); 
        }
        protected override NodeStatus OnEvaluate(Transform transform, Blackboard blackboard)
        {
            var childNodeStatus = children[0].Evaluate(transform, blackboard);

            return childNodeStatus switch
            {
                NodeStatus.Success => NodeStatus.Failure,
                NodeStatus.Failure => NodeStatus.Success,
                _ => NodeStatus.Running,
            };
        }

      
    }
}
