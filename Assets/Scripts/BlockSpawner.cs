using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////Script attached to Grid gameobject. Spawns a grid to be used.

public class BlockSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Grid
    {
        public int gridSizeX;
        public int gridSizeZ;
        public float gridSpacingOffsetX = 1f;
        public float gridSpacingOffsetZ = 1f;
        public Vector3 gridOrigin = Vector3.zero;
        [System.NonSerialized] public List<int> randomList = new List<int>();
    }

    public int playerCount = 4;
    public List<Grid> grids;

    private void Start()
    {
        //spawnSelectedGrid(1);
    }

    public void spawnSelectedGrid(int gridIndex)
    {
        randomizeSpawnPoint(grids[gridIndex]);

        for (int i = 1; i < playerCount + 1; i++)
        {
            spawnAllBlocksForPlayer(grids[gridIndex], i);
        }
    }

    private void randomizeSpawnPoint(Grid grid) {

        List<int> uniqueNumbers = new List<int>();

        for (int i = 0; i < grid.gridSizeX * grid.gridSizeZ; i++)
        {
            uniqueNumbers.Add(i);
        }
        for (int i = 0; i < grid.gridSizeX * grid.gridSizeZ; i++)
        {
            int ranNum = uniqueNumbers[Random.Range(0, uniqueNumbers.Count)];
            grid.randomList.Add(ranNum);
            uniqueNumbers.Remove(ranNum);
        }
    }

    private void spawnAllBlocksForPlayer(Grid grid, int playerNum)
    {
        var longNum = grid.randomList.Count / playerCount;

        for (int i= (playerNum-1) * longNum; i< playerNum * longNum; i++)
        {
            var x = Mathf.FloorToInt(grid.randomList[i]/grid.gridSizeZ);
            var z = grid.randomList[i] % grid.gridSizeZ;
                       
            Vector3 spawnPosition = new Vector3(x * grid.gridSpacingOffsetX, 0, z * grid.gridSpacingOffsetZ) + grid.gridOrigin;
            ObjectPooler.instance.SpawnFromPool("Player"+ playerNum +"Blocks", spawnPosition, Quaternion.identity, true);
        }


    }
}