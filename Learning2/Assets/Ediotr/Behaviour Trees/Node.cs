using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public enum Status
    {
        Failure, Success, Running
    }
    public abstract class Node
    {
       
        protected Node fathor;
        protected List<Node> children = new();
        private Status status = Status.Failure;
        public Status Status { get => status; protected set => status = value; }

        public Node()
        {
            fathor = null;
        }

        public Status Evaluate(Transform transform , Blackboard blackboard)
        {
            
            status = OnEvaluate( transform , blackboard);

            return status;
        }
        /// <summary>
        /// 评估节点
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="blackboard"></param>
        /// <returns></returns>
        protected abstract Status OnEvaluate(Transform transform, Blackboard blackboard);

        /// <summary>
        /// 重置节点
        /// </summary>
        public virtual void Reset()
        {
            Status = Status.Failure;

            foreach (var child in children)
            {
                child.Reset();
                
            }
        }
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(Node child)
        {
            child.fathor = this;
            children.Add(child);
            
        }

    }
}
