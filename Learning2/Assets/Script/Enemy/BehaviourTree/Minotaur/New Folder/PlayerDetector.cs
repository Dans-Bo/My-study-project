using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerDetector : MonoBehaviour
{
    private BehaviourTrees.Blackboard blackboard;

    void Start()
    {
        blackboard = GetComponent<BehaviourTrees.Blackboard>();
        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 5f; // 感知范围
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            blackboard.Set("isPlayerDetected", true);
            blackboard.Set("playerTransform", other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            blackboard.Set("isPlayerDetected", false);
            blackboard.Set<Transform>("playerTransform", null);
        }
    }
}