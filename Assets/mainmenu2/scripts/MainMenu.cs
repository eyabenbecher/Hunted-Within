using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject controls;
    public void Play()
    {
        Debug.Log("scene loading");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("player has quit");
    }


    public void Options() 
    { 
        settingMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void settings()
    {
        controls.SetActive(false);
        options.SetActive(true);
    }
    public void Controls()
    {
        controls.SetActive(true);
        options.SetActive(false);
    }

    public void Return()
    {
        settingMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


}
