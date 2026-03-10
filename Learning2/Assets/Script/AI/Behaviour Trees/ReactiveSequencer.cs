using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    /// <summary>
    /// 反应式序列器
    /// 每次评估都从第一次节点开始重新检查
    /// 任何子节点返回失败，则立即中断并返回失败
    /// </summary>
    public class ReactiveSequencer : Node
    {
        private readonly List<Node> nodes;

        public ReactiveSequencer(List<Node> nodes)
        {
            this.nodes = nodes;
        }
        protected override NodeStatus OnEvaluate(Transform transform, Blackboard blackboard)
        {
            bool anyRunning = false;

            foreach (var node in nodes)
            {
                var status = node.Evaluate(transform, blackboard);

                switch (status)
                {
                    case NodeStatus.Success:
                        continue;
                    case NodeStatus.Failure:
                        return NodeStatus.Failure;
                    case NodeStatus.Running:
                        anyRunning = true;
                        break;
                }
            }
            return anyRunning ? NodeStatus.Running : NodeStatus.Success; //有一个running，则整体running，否则全部返回成功
        }
    }
}
