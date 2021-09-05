using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterLevelSpawnBlocks : MonoBehaviour
{
    [SerializeField] private Door[] doorsToOpen;
    [SerializeField] private BlockSpawner blockSpawner;
    public int gridToSpawn;
    private bool once;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player") && !collision.gameObject.tag.Contains("Block") && !once)
        {
            for (int i = 0; i < doorsToOpen.Length; i++)
            {
                doorsToOpen[i].openDoor();
            }

            blockSpawner.spawnSelectedGrid(gridToSpawn);
            once = true;
        }
    }
}
