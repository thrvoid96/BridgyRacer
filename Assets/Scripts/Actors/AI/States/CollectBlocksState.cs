using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CollectBlocksState : IState
{
    private Animator _animator;
    private AIPlayer _aIPlayer;
    private BlockSpawner _blockSpawner;
    private NavMeshAgent _navMeshAgent;

    private RaycastHit hit;
    private LayerMask blockMask;

    private List<Vector3> destinations = new List<Vector3>();

    public CollectBlocksState(AIPlayer aIPlayer, Animator animator, NavMeshAgent navMeshAgent, BlockSpawner blockSpawner)
    {
        _animator = animator;
        _aIPlayer = aIPlayer;
        _navMeshAgent = navMeshAgent;
        _blockSpawner = blockSpawner;
    }

    public void OnEnter()
    {
        _animator.SetFloat("vertical", 1f);
        _animator.SetFloat("idleTime", 0f);
        blockMask = LayerMask.GetMask("Block");
        var list = _blockSpawner.getBlockPositionsForPlayer(_aIPlayer.currentGrid, Int32.Parse(_aIPlayer.gameObject.tag.Replace("Player", "")));

        for (int i=0; i< list.Count; i++)
        {
            destinations.Add(list[i]);
        }

        destinations = destinations.OrderBy(i => Guid.NewGuid()).ToList();

        GotoNextPoint();
    }

    public void OnExit()
    {
        destinations.Clear();
    }

    public void Tick()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.1f)
        {
            GotoNextPoint();
        }
            
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (destinations.Count == 0)
        {
            return;
        }

        //If AI collided with blocks on the way, remove them from destinations list if they're not there anymore

        //Debug.DrawRay(destinations[0] + new Vector3(0f, 0.5f, 0f), Vector3.down * 1f, Color.green, 10f);
        if (!Physics.Raycast(destinations[0] + new Vector3(0f, 1f, 0f), Vector3.down, out hit, 1f, blockMask))
        {
            destinations.RemoveAt(0);
            return;
        }

        // Set the agent to go to the currently selected destination
        _navMeshAgent.destination = destinations[0];
        //Debug.LogError(destinations[0]);

        // Remove reached destination
        destinations.RemoveAt(0);

    }

}
