using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When a player enters a level, spawn the grid and open doors accordingly. Script on each level.

public class DoorAndGridManager : MonoBehaviour
{
    [SerializeField] private Door[] doorsToOpen;
    [SerializeField] private BlockSpawner blockSpawner;
    public int gridToSpawn;
    private bool once;


    private void OnCollisionEnter(Collision collision)
    {
        if (!once)
        { 
            if (collision.gameObject.tag.Contains("Player") && !collision.gameObject.tag.Contains("Block"))
            {
                for (int i = 0; i < doorsToOpen.Length; i++)
                {
                    doorsToOpen[i].openDoor();
                }

                blockSpawner.spawnSelectedGrid(gridToSpawn);
                this.enabled = false;
                once = true;
            }
        }
  }  
}
