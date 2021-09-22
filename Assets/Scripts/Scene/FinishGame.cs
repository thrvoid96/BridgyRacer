using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;
using System.Linq;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private List<CommonBehaviours> playersOnScene;
    [SerializeField] private UIManager uIManager;
    private List<GameObject> lastBlocks = new List<GameObject>();
    private List<GameObject> sortedBlocks = new List<GameObject>();
    private List<float> positions = new List<float>();
   
    //Add every position into a list, sort and send it to ui manager.
    public void EndGame()
    {
        for (int i = 0; i < playersOnScene.Count; i++)
        {
            if(playersOnScene[i].lastBlockPlaced != null)
            {
                lastBlocks.Add(playersOnScene[i].lastBlockPlaced);
                positions.Add(playersOnScene[i].lastBlockPlaced.transform.position.y);
            }            
        }

        for (int i=0; i< lastBlocks.Count; i++)
        { 
            var num = positions.Max();
            var index = positions.IndexOf(num);
            sortedBlocks.Add(lastBlocks[index]);
            positions.RemoveAt(index);
            lastBlocks.RemoveAt(index);
        }

        while (sortedBlocks.Count < 3)
        {
            var nullGameobj = new GameObject();
            sortedBlocks.Add(nullGameobj);
        }

        uIManager.setEndTexts("1st place: " + (sortedBlocks[0].GetComponent<StairBlock>()? "Player" : " ") + " " + sortedBlocks[0].GetComponent<StairBlock>()?.blockNum, "2nd place: " + (sortedBlocks[1].GetComponent<StairBlock>() ? "Player" : " ") + " " + sortedBlocks[1].GetComponent<StairBlock>()?.blockNum, "3rd place: " + (sortedBlocks[2].GetComponent<StairBlock>() ? "Player" : " ") + " " + sortedBlocks[2].GetComponent<StairBlock>()?.blockNum);
    }
}
