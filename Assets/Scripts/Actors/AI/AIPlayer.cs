using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;
using UnityEngine.AI;
using System;

public class AIPlayer : CommonBehaviours
{
    [SerializeField] private Transform finalPosition;
    [SerializeField] private BlockSpawner blockSpawner;

    public int currentGrid = 0;
    private bool collecting;
    private bool once;
    private GameObject lastFloor;

    private NavMeshAgent navMeshAgent;
    private StateMachine _stateMachine;
    

    protected override void Awake()
    {
        base.Awake();
        _stateMachine = new StateMachine();
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    protected override void Update()
    {
        base.Update();

        //Weird bug because navmeshagent with rigiddbody collisions sometimes send AI's flying.
        if (gameObject.transform.position.y <= -5f)
        {
            navMeshAgent.enabled = false;
            this.enabled = false;
        }

        _stateMachine.Tick();
    }

    protected override void Start()
    {
        base.Start();

        var collectBlocks = new CollectBlocksState(this, animator, navMeshAgent, blockSpawner);
        var goTowardsEnd = new GoTowardsEndState(this, animator, navMeshAgent, finalPosition);
        var idleState = new IdleState(this, animator, navMeshAgent);
        var falling = new FallingState(this, animator, navMeshAgent);

        At(collectBlocks, goTowardsEnd, tooFewBlocks(false));
        At(idleState, collectBlocks, tooFewBlocks(true));
        At(goTowardsEnd, collectBlocks, ZeroBlocks());
        At(falling, collectBlocks, Falling(false));

        _stateMachine.AddAnyTransition(falling, Falling(true));


        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

        _stateMachine.SetState(idleState);

        Func<bool> tooFewBlocks(bool value)
        {
            return delegate
            {
                var canCollect = blockStack.Count < 8 && collecting;

                return value
                ? canCollect
                : !canCollect;
            };
        }

        Func<bool> ZeroBlocks() => () => blockStack.Count == 0;

        Func<bool> Falling(bool value) {

            return delegate
            {
                var Fall = isFalling;

                return value
                    ? Fall
                    : !Fall;
            };
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor" && collision.gameObject != lastFloor)
        {
            if (once)
            {
                currentGrid++;
            }

            lastFloor = collision.gameObject;                    
            collecting = true;
            once = true;
        }
    }

}
