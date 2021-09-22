using Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When a player enters a level, spawn the grid and open doors accordingly. Script on each level gameobject.

public class DoorAndGridManager : MonoBehaviour
{
    [SerializeField] private Door[] doorsToOpen;
    [SerializeField] private BlockSpawner blockSpawner;
    [SerializeField] private int playerCount = 5;
    [SerializeField] private int gridToSpawn;

    private List<bool> blocksSpawned = new List<bool>();

    private void Start()
    {
        for (int i = 0; i < playerCount; i++)
        {
            blocksSpawned.Add(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {             
            var player = collision.gameObject.GetComponent<CommonBehaviours>();
            if (!blocksSpawned[player.getPlayerNum])
            {
                blocksSpawned[player.getPlayerNum] = true;

                for (int i = 0; i < doorsToOpen.Length; i++)
                {
                    doorsToOpen[i].openDoor();
                }

                blockSpawner.spawnAllBlocksForPlayer(player.getPlayerNum, gridToSpawn);
            }
        }
    }  
}
