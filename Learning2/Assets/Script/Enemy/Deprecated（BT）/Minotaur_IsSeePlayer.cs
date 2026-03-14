using System;
using BehaviourTrees;
using UnityEngine;

public class Minotaur_IsSeePlayer : Node
{
    private readonly Func<bool> isSeePlayer;
    public Minotaur_IsSeePlayer(Func<bool> isSeePlayer)
    {
        this.isSeePlayer = isSeePlayer;
    }
    protected override NodeStatus OnEvaluate(Transform transform, Blackboard blackboard)
    {
        return isSeePlayer() ? NodeStatus.Success : NodeStatus.Failure;
        
    }

}
