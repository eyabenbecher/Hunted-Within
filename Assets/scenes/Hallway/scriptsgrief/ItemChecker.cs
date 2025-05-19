using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public InventoryObject playerInventory;
    public ItemObject item1;
    public ItemObject item2;
    public ItemObject item3;

    public AudioClip item1Audio;
    public AudioClip item2Audio;
    public AudioClip item3Audio;
    public AudioClip doorAudio;

    public GameObject doorToDeactivate;
    public GameObject canvasToActivate;

    private AudioSource audioSource;

    private bool item1Used = false;
    private bool item2Used = false;
    private bool item3Used = false;
    private bool doorEventTriggered = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (!item1Used && CheckAndRemoveItem(item1))
        {
            PlayAudio(item1Audio);
            item1Used = true;
        }
        else if (!item2Used && CheckAndRemoveItem(item2))
        {
            PlayAudio(item2Audio);
            item2Used = true;
        }
        else if (!item3Used && CheckAndRemoveItem(item3))
        {
            PlayAudio(item3Audio);
            item3Used = true;
        }

        if (item1Used && item2Used && item3Used && !doorEventTriggered)
        {
            doorEventTriggered = true;
            StartCoroutine(HandleDoorEvent());
        }
    }

    private bool CheckAndRemoveItem(ItemObject item)
    {
        foreach (var slot in playerInventory.container)
        {
            if (slot.item == item && slot.amount > 0)
            {
                slot.amount--;
                return true;
            }
        }
        return false;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    private System.Collections.IEnumerator HandleDoorEvent()
    {
        // Wait until the current audio finishes
        while (audioSource.isPlaying)
            yield return null;

        if (canvasToActivate != null)
        {
            canvasToActivate.SetActive(true);
            Destroy(canvasToActivate, 5f); // destroy canvas after 5 seconds
        }

        if (doorToDeactivate != null)
            doorToDeactivate.SetActive(false);

        if (doorAudio != null)
            audioSource.PlayOneShot(doorAudio);
    }
}
