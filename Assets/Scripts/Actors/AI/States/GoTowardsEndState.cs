using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoTowardsEndState : IState
{
    private Animator _animator;
    private AIPlayer _aIPlayer;
    private NavMeshAgent _navMeshAgent;
    private Transform _finalPos;


    public GoTowardsEndState(AIPlayer aIPlayer, Animator animator, NavMeshAgent navMeshAgent, Transform finalPos)
    {
        _animator = animator;
        _aIPlayer = aIPlayer;
        _navMeshAgent = navMeshAgent;
        _finalPos = finalPos;
    }

    public void OnEnter()
    {
        _animator.SetFloat("vertical", 1f);
        _animator.SetFloat("idleTime", 0f);
        _navMeshAgent.SetDestination(_finalPos.position);
    }

    public void OnExit()
    {
        _navMeshAgent.ResetPath();
    }

    public void Tick()
    {
    }
}
