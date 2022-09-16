using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicEndingToMenu : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadScene(1); //hardcoded, scene 1 should be the main menu
    }
}
