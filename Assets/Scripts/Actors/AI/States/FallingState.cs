using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FallingState : IState
{
    private Animator _animator;
    private AIPlayer _aIPlayer;
    private NavMeshAgent _navMeshAgent;

    public FallingState(AIPlayer aIPlayer, Animator animator, NavMeshAgent navMeshAgent)
    {
        _animator = animator;
        _aIPlayer = aIPlayer;
        _navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        _animator.SetTrigger("isFalling");
        _navMeshAgent.enabled = false;
    }

    public void OnExit()
    {
        _animator.SetTrigger("fallingComplete");
        _navMeshAgent.enabled = true;
    }

    public void Tick()
    {
    }
}
