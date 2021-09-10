using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ESCMenu escMenu;
    
    public void ResetScene()
    {
        escMenu.RestartScene();
    }

    public void endGame()
    {
        escMenu.gameEnd();
    }

    public void setEndTexts(string firstPlace, string secondPlace, string thirdPlace)
    {
        escMenu.setFinalTexts(firstPlace,secondPlace,thirdPlace);
    }
}
