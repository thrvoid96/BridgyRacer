using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private Animator _animator;
    private AIPlayer _aIPlayer;
    private NavMeshAgent _navMeshAgent;

    public float followTime;

    public IdleState(AIPlayer aIPlayer, Animator animator, NavMeshAgent navMeshAgent)
    {
        _animator = animator;
        _aIPlayer = aIPlayer;
        _navMeshAgent = navMeshAgent;
    }

    public void Tick()
    {

    }

    public void OnEnter()
    {
        _animator.SetFloat("vertical", 0f);
        _animator.SetFloat("idleTime", 1f);
        _navMeshAgent.enabled = false;
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = true;
    }
}
