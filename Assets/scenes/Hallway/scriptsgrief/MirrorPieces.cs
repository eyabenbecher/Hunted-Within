using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPieces : MonoBehaviour
{
    public InventoryObject playerInventory;
    public ItemObject mirrorItem;

    public AudioSource audioSource;
    public AudioClip firstClip;   // Sound to play when revealed
    public AudioClip secondClip;  // Sound to play after first ends

    private MeshRenderer meshRenderer;
    private bool used = false; // prevent double activation

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
            meshRenderer.enabled = false;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (used) return; // prevent re-use

        if (HasPiece())
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
                RemovePiece();
                used = true;

                // Start audio sequence
                if (audioSource != null && firstClip != null && secondClip != null)
                {
                    StartCoroutine(PlayAudioSequence());
                }
            }
        }
        else
        {
            Debug.Log("You don't have a mirror piece.");
        }
    }

    IEnumerator PlayAudioSequence()
    {
        audioSource.clip = firstClip;
        audioSource.Play();
        yield return new WaitForSeconds(firstClip.length);

        if (secondClip != null)
        {
            audioSource.clip = secondClip;
            audioSource.Play();
        }
    }

    private bool HasPiece()
    {
        foreach (var slot in playerInventory.container)
        {
            if (slot.item == mirrorItem && slot.amount > 0)
            {
                return true;
            }
        }
        return false;
    }

    private void RemovePiece()
    {
        foreach (var slot in playerInventory.container)
        {
            if (slot.item == mirrorItem && slot.amount > 0)
            {
                slot.amount--;
                break;
            }
        }
    }
}
