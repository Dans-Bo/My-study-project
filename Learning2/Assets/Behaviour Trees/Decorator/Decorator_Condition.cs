using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public class Decorator_Condition : Node
    {
        private readonly Func<Transform, Blackboard, bool> condition;

        public Decorator_Condition(Func<Transform, Blackboard, bool> condition)
        {
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
        } 
        
        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            return condition.Invoke(transform,blackboard)? Status.Success:Status.Failure;
        }  
    }
}
