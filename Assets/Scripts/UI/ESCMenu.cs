using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ESCMenu : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    [SerializeField] private TextMeshProUGUI firstPlaceText,secondPlaceText,ThirdPlaceText;
    private bool menuOpen;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuOpen)
            {
                setMenuOpen(true);
            }
            else
            {
                setMenuOpen(false);
            }
        }
    }

    public void setFinalTexts(string firstPlace, string secondPlace, string thirdPlace)
    {
        firstPlaceText.text = firstPlace;
        secondPlaceText.text = secondPlace;
        ThirdPlaceText.text = thirdPlace;

        gameEnd();
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void gameEnd()
    {
        setMenuOpen(true);
        Time.timeScale = 0f;
    }

    private void setMenuOpen(bool value)
    {
        backGround.SetActive(value);
        menuOpen = value;
    }
}
