using UnityEngine;

namespace BehaviourTrees
{
    [RequireComponent(typeof(Blackboard))]
    public class BehaviourTree : MonoBehaviour
    {
        private Node root;
        public Node Root { get => root; protected set => root = value; }

        private Blackboard blackboard;
        public Blackboard Blackboard { get => blackboard; protected set => blackboard = value; }
        

         void Awake()
        {
            blackboard = GetComponent<Blackboard>();

            OnSetup();  
        }
        

        void Update()
        {
            root?.Evaluate(gameObject.transform, blackboard);
        }
        /// <summary>
        /// 行为树构建
        /// </summary>
        protected virtual void OnSetup()
        {
            
        }
    }
}
