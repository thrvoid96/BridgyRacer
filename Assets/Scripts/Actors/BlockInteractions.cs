using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;

//Any Block and Player related interaction

public class BlockInteractions
{
    public void addBlockToStack(CommonBehaviours playerBehaviours, Block block, Transform stackStart, Vector3 gapBetween, Stack<Block> blockStack)
    {
        block.disableAnimator();

        block.moveTowards(stackStart, playerBehaviours.nextBlockPosition);

        playerBehaviours.nextBlockPosition += gapBetween;

        blockStack.Push(block);
    }

    public void placeBlock(CommonBehaviours playerBehaviours, GameObject block, Material playerMat,  string playerTag, Vector3 gapBetween, Stack<Block> blockStack, GameObject playerStopper)
    {
        if (blockStack.Count > 0)
        {
            block.GetComponent<MeshRenderer>().material = playerMat;
            block.tag = playerTag + "Block";
            playerBehaviours.lastBlockPlacedPosition = block.transform.position;

            var poppedObj = blockStack.Pop();
            poppedObj.respawnCube(playerTag + "Blocks");
            poppedObj.Inactivate();

            playerBehaviours.nextBlockPosition -= gapBetween;

        }
        else
        {
            playerStopper.SetActive(true);
            playerStopper.transform.position = new Vector3(block.gameObject.transform.position.x, block.gameObject.transform.position.y, block.gameObject.transform.position.z - 0.1f);
        }
    }

    public void loseAllBlocks(CommonBehaviours playerBehaviours,string playerTag, Stack<Block> blockStack)
    {
        playerBehaviours.nextBlockPosition = new Vector3(0, 0, 0);
        var count = blockStack.Count;

        for (int i = 0; i < count; i++)
        {
            var block = blockStack.Pop();
            block.respawnCube(playerTag + "Blocks");

            ObjectPooler.instance.SpawnFromPool("GreyBlocks", block.transform.position, block.transform.rotation, true);
            block.transform.parent = null;
            block.Inactivate();
        }
    }
}
