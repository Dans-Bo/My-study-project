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
        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            var childStatus = children[0].Evaluate(transform, blackboard);

            return childStatus switch
            {
                Status.Success => Status.Failure,
                Status.Failure => Status.Success,
                _ => Status.Running,
            };
        }

      
    }
}
