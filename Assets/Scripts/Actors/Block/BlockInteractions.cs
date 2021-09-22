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

    public void placeBlock(CommonBehaviours playerBehaviours, StairBlock stairBlock, Vector3 gapBetween, Stack<Block> blockStack, GameObject playerStopper)
    {
        if (blockStack.Count > 0)
        {
            stairBlock.blockNum = playerBehaviours.getPlayerNum;
            stairBlock.blockMaterial = playerBehaviours.getMaterial;
            stairBlock.doStairEffects();

            playerBehaviours.lastBlockPlaced = stairBlock.gameObject;

            var poppedObj = blockStack.Pop();
            poppedObj.respawnCube("Player" + playerBehaviours.getPlayerNum + "Blocks");
            poppedObj.Inactivate();

            playerBehaviours.nextBlockPosition -= gapBetween;

        }
        else
        {
            playerStopper.SetActive(true);
            playerStopper.transform.position = new Vector3(stairBlock.gameObject.transform.position.x, stairBlock.gameObject.transform.position.y, stairBlock.gameObject.transform.position.z - 0.1f);
        }
    }

    public void loseAllBlocks(CommonBehaviours playerBehaviours, Stack<Block> blockStack)
    {
        playerBehaviours.nextBlockPosition = new Vector3(0, 0, 0);
        var count = blockStack.Count;

        for (int i = 0; i < count; i++)
        {
            var block = blockStack.Pop();
            block.respawnCube("Player" + playerBehaviours.getPlayerNum + "Blocks");

            ObjectPooler.instance.SpawnFromPool("GreyBlocks", block.transform.position, block.transform.rotation, true);
            block.transform.parent = null;
            block.Inactivate();
        }
    }
}
