using System.Collections.Generic;
using UnityEngine;

public class SequentialActivator : MonoBehaviour
{
    public List<GameObject> items; // Assign this list from the inspector

    private int currentIndex = 0;

    void Start()
    {
        // Deactivate all items except the first one
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
                items[i].SetActive(i == currentIndex);
        }
    }

    void Update()
    {
        // If the current item is null or destroyed
        if (currentIndex < items.Count && items[currentIndex] == null)
        {
            currentIndex++;

            if (currentIndex < items.Count && items[currentIndex] != null)
            {
                items[currentIndex].SetActive(true);
            }
        }
    }
}
