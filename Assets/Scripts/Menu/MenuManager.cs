using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject TitleScreenCanvas;
    public GameObject ButtonCanvas;

    private bool pressedButton = false; // if a button has been pressed to start the game menu

    void Start()
    {
        TitleScreenCanvas.SetActive(true);
        ButtonCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // press any button 
        if (!pressedButton && Input.anyKeyDown)
        {
            pressedButton = true; // button has been pressed so true
            TitleScreenCanvas.SetActive(false); // set to not active 
            ButtonCanvas.SetActive(true); // set to active
        }

        // escape takes you back to the main menu
        if (pressedButton && Input.GetKeyDown(KeyCode.Escape))
        {
            pressedButton = false;
            TitleScreenCanvas.SetActive(true);
            ButtonCanvas.SetActive(false);
        }


    }
}