using UnityEngine;
using System.Collections;

public class LiaChairSit : MonoBehaviour
{
    public Transform sitPoint;
    public GameObject lia;
    public Animator liaAnimator;

    public GameObject sitPromptUI;
    public GameObject mirrorObject;
    public GameObject chloeObject;

    public DialogueCanvasManager dialogueManager; // Assign this in inspector

    private bool isPlayerInZone = false;
    private bool hasSat = false;
    private Collider liaCollider;
    private Quaternion lockedRotation;

    void Start()
    {
        if (sitPromptUI != null)
            sitPromptUI.SetActive(false);

        if (chloeObject != null)
            chloeObject.SetActive(false);

        liaCollider = lia.GetComponent<Collider>();
        if (liaCollider == null)
            liaCollider = lia.GetComponentInChildren<Collider>();
    }

    void Update()
    {
        if (isPlayerInZone && !hasSat)
        {
            if (sitPromptUI != null && !sitPromptUI.activeSelf)
                sitPromptUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                SitLia();
                if (sitPromptUI != null)
                    sitPromptUI.SetActive(false);
            }
        }

        if (hasSat)
        {
            lia.transform.rotation = lockedRotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInZone = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInZone = false;
    }

    void SitLia()
    {
        hasSat = true;

        lia.transform.position = sitPoint.position;
        lockedRotation = sitPoint.rotation;
        lia.transform.rotation = lockedRotation;

        if (liaCollider != null)
            liaCollider.enabled = false;

        if (liaAnimator != null)
        {
            liaAnimator.Play("Sit");
            liaAnimator.applyRootMotion = true;
        }

        StartCoroutine(TriggerChloeAppearanceAfterDelay());
    }

    IEnumerator TriggerChloeAppearanceAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        if (mirrorObject != null)
            mirrorObject.SetActive(false);

        if (chloeObject != null)
            chloeObject.SetActive(true);

        // Start first dialogue canvas
        if (dialogueManager != null)
        {
            dialogueManager.StartConversation(); // ✅ Correct method name
        }
    }
}
