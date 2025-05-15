using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerPass : MonoBehaviour
{
    public InventoryObject playerInventory;
    public ItemObject pictureItem;

    public GameObject objectToAppear; // Object to show or spawn
    public Transform spawnPoint;      // Where to show/spawn the object

    public Transform doorToMove;      // Door to move
    public float moveDistance = 2f;   // How far on Z axis
    public float moveSpeed = 1f;      // Speed of movement

    private bool isMoving = false;
    private bool hasActivated = false; // Prevent multiple activations
    private Vector3 doorTargetPosition;

    private void OnMouseDown()
    {
        if (hasActivated) return;

        if (HasPicture())
        {
            hasActivated = true;

            // Remove picture from inventory
            RemovePicture();

            // Appear object
            if (objectToAppear != null && spawnPoint != null)
            {
                Instantiate(objectToAppear, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning("Object or spawn point not assigned!");
            }

            // Move door
            if (doorToMove != null)
            {
                doorTargetPosition = doorToMove.position + new Vector3(0, 0, moveDistance);
                StartCoroutine(MoveDoor());
            }
            else
            {
                Debug.LogWarning("Door Transform not assigned!");
            }
        }
        else
        {
            Debug.Log("Player doesn't have the required picture.");
        }
    }

    private IEnumerator MoveDoor()
    {
        isMoving = true;
        while (Vector3.Distance(doorToMove.position, doorTargetPosition) > 0.01f)
        {
            doorToMove.position = Vector3.MoveTowards(doorToMove.position, doorTargetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }

    private bool HasPicture()
    {
        foreach (var slot in playerInventory.container)
        {
            if (slot.item == pictureItem && slot.amount > 0)
            {
                return true;
            }
        }
        return false;
    }

    private void RemovePicture()
    {
        foreach (var slot in playerInventory.container)
        {
            if (slot.item == pictureItem && slot.amount > 0)
            {
                slot.amount--;
                break;
            }
        }
    }
}
