using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is not the main menu (by name or build index)
        if (scene.name != "mainmenu2" )
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
