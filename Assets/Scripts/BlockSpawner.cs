using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////Script attached to Grid gameobject. Spawns a grid to be used.

public class BlockSpawner : MonoBehaviour
{
    public int gridSizeX;
    public int gridSizeZ;
    public float gridSpacingOffsetX = 1f;
    public float gridSpacingOffsetZ = 1f;
    public Vector3 gridOrigin = Vector3.zero;


    private void Start()
    {
        SpawnBlocks();
    }

    private void SpawnBlocks()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffsetX, 0, z * gridSpacingOffsetZ) + gridOrigin;
                PickAndSpawn(spawnPosition, Quaternion.identity);
            }
        }
    }

    private void PickAndSpawn(Vector3 positionToSpawn, Quaternion rotationToSpawn)
    {
        ObjectPooler.instance.SpawnFromPool("Block", positionToSpawn, rotationToSpawn);
    }

}