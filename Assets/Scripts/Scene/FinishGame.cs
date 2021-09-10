using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private List<CommonBehaviours> playersOnScene;
    [SerializeField] private UIManager uIManager;
    private List<float> lastPositions = new List<float>();
    private List<float> list = new List<float>();
   
    //Add every position into a list, sort and send it to ui manager.
    public void EndGame()
    {
        for (int i = 0; i < playersOnScene.Count; i++)
        {
           
            lastPositions.Add(playersOnScene[i].lastBlockPlacedPosition.y);
            lastPositions[i] = (Mathf.FloorToInt(lastPositions[i]) * 10) + i;
        }

        lastPositions.Sort();

        list.Capacity = lastPositions.Count;

        for (int i = 0; i < lastPositions.Count; i++)
        {
            for (int x = 0; x < lastPositions.Count; i++)
            {
                if((lastPositions[x] - i) % 10 == 0)
                {
                    list.Insert(x, i);
                }
            }
        }

        uIManager.setEndTexts("Player" + list[0].ToString() + " " + "wins!", "2nd place:" + " " + "Player" + list[1].ToString(), "3rd place:" + " " + "Player" + list[2].ToString());
    }
}
