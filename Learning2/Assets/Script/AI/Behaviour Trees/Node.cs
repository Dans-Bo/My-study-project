using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    /// <summary>
    /// 行为树节点状态
    /// </summary>
    public enum NodeStatus
    {
        Failure, Success, Running
    }

    /// <summary>
    /// 行为树
    /// </summary>
    public abstract class Node
    { 
        protected Node parent; //父节点
        protected readonly List<Node> children = new();  //子节点列表
        private NodeStatus currentStatus = NodeStatus.Failure;
        public NodeStatus CurrentStatus { get => currentStatus; protected set => currentStatus = value; }

        public Node()
        {
            parent = null;
        }

        public Node (List<Node> children)
        {
            if (children == null) return;

            foreach (Node child in children)
            {
                AddChild(child);
            }
        }
/// <summary>
/// 评估节点
/// </summary>
/// <param name="transform"></param>
/// <param name="blackboard"></param>
/// <returns></returns>
        public NodeStatus Evaluate(Transform transform , Blackboard blackboard)
        {
            
            currentStatus = OnEvaluate( transform , blackboard);

            return currentStatus;
        }
        
        protected abstract NodeStatus OnEvaluate(Transform transform, Blackboard blackboard);

        /// <summary>
        /// 重置节点
        /// </summary>
        public virtual void Reset()
        {
            currentStatus = NodeStatus.Failure;

            foreach (var child in children)
            {
                child.Reset();
                #if UNITY_EDITOR
                Debug.Log("已重置子节点");
                #endif
            }
        }
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(Node child)
        {
            if (child == null || children.Contains(child)) return;
            
            child.parent = this;
            children.Add(child);
            #if UNITY_EDITOR
                Debug.Log("已添加子节点");
                #endif
        }

    }
}
