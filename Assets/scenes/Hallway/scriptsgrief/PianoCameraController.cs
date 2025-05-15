using System.Collections;
using UnityEngine;

public class PianoKeyController : MonoBehaviour
{
    public AudioClip doNote;
    public AudioClip reNote;
    public AudioClip miNote;
    public AudioClip faNote;
    public AudioClip solNote;
    public AudioClip laNote;
    public AudioClip siNote;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            if (hits.Length == 0) return;

            Vector3 mousePos = Input.mousePosition;

            float closestScreenDist = Mathf.Infinity;
            GameObject closestObject = null;

            foreach (var hit in hits)
            {
                if (IsValidNoteTag(hit.transform.gameObject.tag))
                {
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(hit.transform.position);
                    float dist = Vector2.Distance(new Vector2(screenPoint.x, screenPoint.y), new Vector2(mousePos.x, mousePos.y));
                    if (dist < closestScreenDist)
                    {
                        closestScreenDist = dist;
                        closestObject = hit.transform.gameObject;
                    }
                }
            }

            if (closestObject != null)
            {
                StartCoroutine(RotateObjectAndPlayNote(closestObject));
            }
        }
    }

    bool IsValidNoteTag(string tag)
    {
        tag = tag.ToLower();
        return tag == "do" || tag == "re" || tag == "mi" || tag == "fa" || tag == "sol" || tag == "la" || tag == "si";
    }

    IEnumerator RotateObjectAndPlayNote(GameObject obj)
    {
        float rotationAmount = 3f;
        float duration = 0.1f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float rotX = Mathf.Lerp(0, rotationAmount, elapsed / duration);
            obj.transform.localEulerAngles = new Vector3(rotX, obj.transform.localEulerAngles.y, obj.transform.localEulerAngles.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            float rotX = Mathf.Lerp(rotationAmount, 0, elapsed / duration);
            obj.transform.localEulerAngles = new Vector3(rotX, obj.transform.localEulerAngles.y, obj.transform.localEulerAngles.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.localEulerAngles = new Vector3(0, obj.transform.localEulerAngles.y, obj.transform.localEulerAngles.z);

        PlayNoteSound(obj.tag);
    }

    void PlayNoteSound(string tag)
    {
        tag = tag.ToLower();

        switch (tag)
        {
            case "do":
                audioSource.PlayOneShot(doNote);
                break;
            case "re":
                audioSource.PlayOneShot(reNote);
                break;
            case "mi":
                audioSource.PlayOneShot(miNote);
                break;
            case "fa":
                audioSource.PlayOneShot(faNote);
                break;
            case "sol":
                audioSource.PlayOneShot(solNote);
                break;
            case "la":
                audioSource.PlayOneShot(laNote);
                break;
            case "si":
                audioSource.PlayOneShot(siNote);
                break;
        }
    }
}
