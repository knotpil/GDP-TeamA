using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void onPlayClick()
    {
        SceneManager.LoadSceneAsync("Test Scene");
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
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void onExitClick()
    {
        Application.Quit();
    }
}