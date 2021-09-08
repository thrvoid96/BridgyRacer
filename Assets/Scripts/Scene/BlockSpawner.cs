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
        [System.NonSerialized] public Dictionary<int,List<Vector3>> blockPositions = new Dictionary<int, List<Vector3>>();
    }

    [SerializeField] private int playerCount = 4;
    public List<Grid> grids;

    private void Start()
    {
        //spawnSelectedGrid(2);
    }

    public List<Vector3> getBlockPositionsForPlayer(int gridIdx, int playerNumber)
    {
        return grids[gridIdx].blockPositions[playerNumber];
    }

    public void spawnSelectedGrid(int gridIndex)
    {
        randomizeSpawnPoint(grids[gridIndex]);

        for (int i = 0; i < playerCount; i++)
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
        var list = new List<Vector3>();

        for (int i= playerNum * longNum; i< (playerNum + 1) * longNum; i++)
        {
            var x = Mathf.FloorToInt(grid.randomList[i]/grid.gridSizeZ);
            var z = grid.randomList[i] % grid.gridSizeZ;
                       
            Vector3 spawnPosition = new Vector3(x * grid.gridSpacingOffsetX, 0, z * grid.gridSpacingOffsetZ) + grid.gridOrigin;
            var obj = ObjectPooler.instance.SpawnFromPool("Player"+ playerNum +"Blocks", spawnPosition, Quaternion.identity, true);
            list.Add(obj.transform.position);
        }

        grid.blockPositions.Remove(playerNum);
        grid.blockPositions.Add(playerNum, list);

    }
}