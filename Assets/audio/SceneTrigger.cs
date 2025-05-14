using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && AudioButtonCycle.hasPlayedAudio)
        {
            
            SceneManager.LoadScene("ZyreathCutScene1");
        }
    }
}
