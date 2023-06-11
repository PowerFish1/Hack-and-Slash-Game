using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    public bool gamePaused = false;
    public GameObject pauseMenu;
    public AudioSource clickSound;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {
            if (!gamePaused)
            {
                Time.timeScale = 0;
                gamePaused = true;
                pauseMenu.SetActive(true);
            }

            else
            {
                gamePaused = false;
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }
        }
    }

    // Resumes game
    public void ResumeGame()
    {
        clickSound.Play();
        pauseMenu.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1;
    }

    // restarts game
    public void RestartLevel()
    {
        clickSound.Play();
        pauseMenu.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    // go to main menu
    public void MainMenu()
    {
        clickSound.Play();
        pauseMenu.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
