
using System.Linq;
using BehaviourTrees;
using UnityEngine;

[RequireComponent(typeof(CheckPlayer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Minotaur_AI : BehaviourTree
{
    private Minotaur_StateMachine stateMachine;
    private Node rootNode;

    protected override void OnSetup()
    {
        stateMachine = GetComponent<Minotaur_StateMachine>();

        Node [] seletcted = {} ;
        
    }
}