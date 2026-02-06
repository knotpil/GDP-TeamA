using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void onPlayClick()
    {
        SceneManager.LoadSceneAsync("Game");
    }


    public void onCreditClick()
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    public void onControlClick()
    {
        SceneManager.LoadSceneAsync("Controls");
    }

    public void onMenuClick()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void onExitClick()
    {
        Application.Quit();
    }
}