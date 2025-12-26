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
        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            bool anyRunning = false;

            foreach (var node in nodes)
            {
                var status = node.Evaluate(transform, blackboard);

                switch (status)
                {
                    case Status.Success:
                        continue;
                    case Status.Failure:
                        return Status.Failure;
                    case Status.Running:
                        anyRunning = true;
                        break;
                }
            }
            return anyRunning ? Status.Running : Status.Success; //有一个running，则整体running，否则全部返回成功
        }
    }
}
