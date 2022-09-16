using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ClickStart()
    {
        //server/lobby connection goes here

        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
    public void ClickCinematic()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
