using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject h2p_Window;

    public void GamePlay()
    {
        PlayerPrefs.SetInt("idx", 0);
        SceneManager.LoadScene("InGameScene");
    }

    public void How2Play()
    {
        h2p_Window.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
