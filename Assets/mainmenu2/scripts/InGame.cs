using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class InGame : MonoBehaviour
{
    public GameObject uiCanvas;
    public MonoBehaviour cameraController; 
    private bool isPaused = false;
    public GameObject optionsMenu;
    public GameObject settingsPanel;
    public GameObject controlPanel;

    void Start()
    {
        optionsMenu.SetActive(false);
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }

        Time.timeScale = 1;
        isPaused = false;
        if (cameraController != null)
            cameraController.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        uiCanvas.SetActive(true);
        if (cameraController != null)
            cameraController.enabled = false;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        uiCanvas.SetActive(false);
        if (cameraController != null)
            cameraController.enabled = true;
        isPaused = false;
        
        optionsMenu.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("mainmenu2");
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
    }
    public void Settings()
    {
        settingsPanel.SetActive(true);
        controlPanel.SetActive(false );
    }
    public void Controls()
    {
        settingsPanel.SetActive(false);
        controlPanel.SetActive(true);
    }

}
