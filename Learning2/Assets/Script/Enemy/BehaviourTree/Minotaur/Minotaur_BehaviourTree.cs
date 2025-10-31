
using System.Linq;
using BehaviourTrees;
using UnityEngine;

[RequireComponent(typeof(CheckPlayer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Minotaur_BehaviourTree : BehaviourTree
{
    [SerializeField] Transform[] wayPoints = null;
    [SerializeField] float moveSpeed = 3f; //巡逻移速
    [SerializeField] float chaseSpeed = 6f;//追击移速
    [SerializeField] float attackRange = 2f; //可以攻击的距离
    [SerializeField] float attackColdown = 2f; //攻击冷却时间
    bool seePlayer = false;
    CheckPlayer checkPlayer;
    
    Animator animator;
    Rigidbody2D rb;

    private void Start() => seePlayer = false;



    void OnDisable()
    {
        checkPlayer.IsSeePlayer -= OnIsSeePlayer;

    }

    private void OnSetPlayerPosition(Vector2 vector)
    {
        if (Blackboard == null)
        {
            Debug.LogError("Blackboard is null in OnSetPlayerPosition");
            return;
        }
        Blackboard.Set("playerPosition", vector);
    }

    private void OnIsSeePlayer(bool obj)
    {
        seePlayer = obj;
        Blackboard.Set("seePlayer", seePlayer);
    }


    protected override void OnSetup()
    {
        animator = GetComponent<Animator>();
        checkPlayer = GetComponent<CheckPlayer>();
        rb = GetComponent<Rigidbody2D>();

        checkPlayer.IsSeePlayer += OnIsSeePlayer; //是否发现玩家事件
        checkPlayer.SetPlayerPosition += OnSetPlayerPosition; //获得玩家位置

        SetToBlackboard();

        var patrol = new Minotaur_PatrolTask(wayPoints);
        var chase = new Minotaur_Chase();
        var _isSeePlayer = new Minotaur_IsSeePlayer(() => seePlayer);
        var attack = new Minotaur_Attack();

        Node[] battleReactiveSequenceNode = { _isSeePlayer, chase, attack };
        var battleReaciveSequencer = new Sequencer(battleReactiveSequenceNode.ToList());

        Node[] rootReactiveSelectorNode = { battleReaciveSequencer, patrol };
        var rootReactiveSelector = new Selector(rootReactiveSelectorNode.ToList());

        Root = rootReactiveSelector;
    }
    
    private void SetToBlackboard()
    {
        Blackboard.Set("moveSpeed", moveSpeed);
        Blackboard.Set("chaseSpeed", chaseSpeed);
        Blackboard.Set("attackRange", attackRange);
        Blackboard.Set("attackColdown", attackColdown);
        Blackboard.Set("animator", animator);
        Blackboard.Set("rb", rb);
        
    }

}