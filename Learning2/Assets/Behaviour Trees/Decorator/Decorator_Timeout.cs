/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public class Decorator_Timeout : Node
    {
        private readonly float timeOutSeconds; //存储超时时间
        private float timeCount; //计时器
        private bool isRunning;

        public Decorator_Timeout(float timeOutSeconds) 
        {
            this.timeOutSeconds = timeOutSeconds; 
        }
        protected override Status OnEvaluate(Transform transform, Blackboard blackboard)
        {
            if (!isRunning)
            {
                isRunning = true;
                timeCount = 0f;
            }

            var childStatus = children[0].Evaluate(transform, blackboard);

            if (childStatus == Status.Running)
            {
                timeCount += Time.deltaTime;

                if (timeCount >= timeOutSeconds)
                {
                    Reset();
                    return Status.Failure;
                }
            }
            else Reset();
            return childStatus;
        }

        protected override void Reset()
        {
            base.Reset();
            isRunning = false;
            timeCount = 0f;
        }

    }
}
 */