using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausing : MonoBehaviour
{
    [SerializeField] bool isPaused = false;
    [SerializeField] GameObject pauseMenu;

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                isPaused = false;
            }
        }
    }
    void Update()
    {
        PauseGame();
    }
}
