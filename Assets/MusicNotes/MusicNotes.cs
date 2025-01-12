using UnityEngine;

public class MusicNotes : MonoBehaviour
{
    public AudioClip doNote;  // Assign the "Do" note audio clip
    public AudioClip reNote;  // Assign the "Re" note audio clip
    public AudioClip miNote;  // Assign the "Mi" note audio clip
    public AudioClip faNote;  // Assign the "Fa" note audio clip
    public AudioClip solNote; // Assign the "Sol" note audio clip
    public AudioClip laNote;  // Assign the "La" note audio clip
    public AudioClip siNote;  // Assign the "Si" note audio clip

    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Methods to play each note
    public void PlayDo()
    {
        PlayNote(doNote);
    }

    public void PlayRe()
    {
        PlayNote(reNote);
    }

    public void PlayMi()
    {
        PlayNote(miNote);
    }

    public void PlayFa()
    {
        PlayNote(faNote);
    }

    public void PlaySol()
    {
        PlayNote(solNote);
    }

    public void PlayLa()
    {
        PlayNote(laNote);
    }

    public void PlaySi()
    {
        PlayNote(siNote);
    }

    private void PlayNote(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for this note.");
        }
    }
}
