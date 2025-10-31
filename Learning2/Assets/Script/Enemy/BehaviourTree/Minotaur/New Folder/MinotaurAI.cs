using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public class MonsterAI : BehaviourTree
    {
        [Header("怪物属性")]
        public float attackRange = 1.5f;
        public float chaseRange = 5f;
        public Transform[] patrolPoints;

        protected override void OnSetup()
        {
            Blackboard.Set("isPlayerDetected", false);
            Blackboard.Set("isHurt", false);
            Blackboard.Set<Transform>("playerTransform", null);
            Blackboard.Set("currentPatrolIndex", 0);

            // 动作节点
            Node patrol = new Action_Patrol(patrolPoints);
            Node chase = new Action_Chase();
            Node attack = new Action_Attack();
            Node hurt = new Action_Hurt();

            // 条件
            var condHurt = new Decorator_Condition((t, bb) => bb.Get<bool>("isHurt"));
            var condSeePlayer = new Decorator_Condition((t, bb) => bb.Get<bool>("isPlayerDetected"));
            var condInAttackRange = new Decorator_Condition((t, bb) =>
            {
                var player = bb.Get<Transform>("playerTransform");
                if (player == null) return false;
                return Vector2.Distance(t.position, player.position) <= attackRange;
            });

            // 行为树结构
            Root = new ReaciveSelector(new List<Node>
            {
                new ReactiveSequencer(new List<Node> { condHurt, hurt }),
                new ReactiveSequencer(new List<Node> {
                    condSeePlayer,
                    new ReaciveSelector(new List<Node> {
                        new ReactiveSequencer(new List<Node> { condInAttackRange, attack }),
                        chase
                    })
                }),
                patrol
            });
        }

        private void Start()
        {
            // 设置 Animator
            var animator = GetComponent<Animator>();
            Blackboard.Set("animator", animator);
        }
    }
}