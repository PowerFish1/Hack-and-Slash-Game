using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    public AudioSource clickSound;

    public void PlayGame()
    {
        clickSound.Play();
        SceneManager.LoadScene(2);
    }

    public void Credits()
    {
        clickSound.Play();
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        clickSound.Play();
        Application.Quit();
    }
}
