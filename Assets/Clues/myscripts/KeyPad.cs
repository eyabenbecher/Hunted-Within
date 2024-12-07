using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeyPad : MonoBehaviour
{
    [SerializeField] private Text not; // Feedback text
    private string ans = "DoReMi"; // Correct answer

    // Reference to Animator components for the three animations
    [SerializeField] private Animator[] animators; // Array of animators for multiple animations

    // Reference to AudioSource components for the two soundtracks
    [SerializeField] private AudioSource[] soundtracks; // Array of AudioSource components for soundtracks

    // Unique trigger names for each animation
    [SerializeField] private string[] animationTriggers = { "PlayAnimation1", "PlayAnimation2", "PlayAnimation3" };

    public void Note(string note)
    {
        not.text += note.ToString();
    }

    public void Execute()
    {
        if (not.text == ans)
        {
            not.text = "CORRECT";
            not.color = Color.green;
            not.fontSize = 20;
            not.rectTransform.localPosition = Vector3.zero;

            // Trigger animations with unique triggers
            TriggerAnimations();

            // Play soundtracks
            PlaySoundtracks();

            // Hide the canvas after a correct answer
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            not.text = "INCORRECT";
            not.color = Color.red;
            not.fontSize = 20;
            not.rectTransform.localPosition = Vector3.zero;
            StartCoroutine(ResetText());
        }
    }

    // Trigger the animations using unique triggers
    private void TriggerAnimations()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i] != null && i < animationTriggers.Length)
            {
                animators[i].SetTrigger(animationTriggers[i]); // Trigger each animation with its own trigger name
            }
        }
    }

    // Play the soundtracks
    private void PlaySoundtracks()
    {
        foreach (AudioSource soundtrack in soundtracks)
        {
            if (soundtrack != null)
            {
                soundtrack.Play(); // Play the soundtrack
            }
        }
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(1f);

        not.text = "";
        not.color = Color.black;
        not.fontSize = 20;
    }
}
