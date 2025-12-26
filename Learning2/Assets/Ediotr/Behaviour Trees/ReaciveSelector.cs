using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    /// <summary>
    /// 反应式选择器
    /// 每次从第一个子节点开始重新检查
    /// 一旦上层优先级分支恢复可用，立即切换
    /// </summary>
    public class ReaciveSelector : Node
    {
        private readonly List<Node> nodes;
        public ReaciveSelector(List<Node> nodes)
        {
            this.nodes = nodes;
        } 

        protected override  Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            foreach (var node in nodes)
            {
                var status = node.Evaluate(transform, blackboard);

                switch (status)
                {
                    case Status.Success:
                        return Status.Success;
                    case Status.Failure:
                        continue;
                    case Status.Running:
                        return Status.Running;
                }
            }
            return Status.Failure;
        }
    }
}
