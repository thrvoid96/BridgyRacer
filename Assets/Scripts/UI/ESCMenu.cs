using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCMenu : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    private bool menuOpen;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuOpen)
            {
                backGround.SetActive(true);
                menuOpen = true;
            }
            else
            {
                backGround.SetActive(false);
                menuOpen = false;
            }
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Level1");
    }

    public void gameEnd()
    {
        backGround.SetActive(true);
        menuOpen = true;
        Time.timeScale = 0f;
    }
}
