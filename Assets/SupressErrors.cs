using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SuppressSpecificErrors : MonoBehaviour { 
void Update()
    {
    GameObject[] objects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (!obj.activeInHierarchy) continue;

            Vector3 pos = obj.transform.position;
    Collider col = obj.GetComponent<Collider>();

            // Check for NaN/Infinity in position
            if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z) ||
                float.IsInfinity(pos.x) || float.IsInfinity(pos.y) || float.IsInfinity(pos.z))
            {
                Debug.LogError($"❌ Invalid Transform: {obj.name} at {pos}", obj);
            }

// Check for extremely large positions
if (pos.magnitude > 10000f)
{
    Debug.LogWarning($"⚠️ Large Position Detected: {obj.name} at {pos}", obj);
}

// Check for invalid colliders
if (col != null && col.bounds.size.magnitude > 10000f)
{
    Debug.LogWarning($"⚠️ Large Collider Detected: {obj.name}", obj);
}
        }
    }
}