using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    public Transform[] pageLeaf;
    public AudioClip openCloseSound;
    public AudioClip pageTurnSound;

    private Transform[] pageAnim;
    private float[] pageAngle;
    private float[] pageAngleMin;
    private float[] pageAngleMax;
    private float speed = 150.0f;
    private int page = -1;
    private int totalPages;
    private AudioSource myAudio;

    [Header("UI & Camera References")]
    public GameObject interactButton;       // Button to show when near book
    public GameObject reactivateButton;     // Button to show when camera disabled
    public Camera mainCamera;
    public Transform player;                 // Assign your player transform here
    public float showButtonDistance = 3f;   // Distance to show interact button

    void Start()
    {
        myAudio = GetComponent<AudioSource>();

        totalPages = pageLeaf.Length;
        pageAnim = new Transform[totalPages];
        pageAngle = new float[totalPages];
        pageAngleMin = new float[totalPages];
        pageAngleMax = new float[totalPages];

        for (int i = 0; i < totalPages; i++)
        {
            pageAngleMin[i] = pageLeaf[i].localEulerAngles.y;
            pageAngleMax[i] = pageLeaf[i].localEulerAngles.y + 170;
            pageAnim[i] = pageLeaf[i].Find("Page");
            if (pageAnim[i] != null)
            {
                pageAnim[i].GetComponent<Animation>()["RL"].speed = 2.0f;
                pageAnim[i].GetComponent<Animation>()["LR"].speed = 2.0f;
            }
        }

        // Make sure buttons start disabled
        interactButton.SetActive(false);
        reactivateButton.SetActive(false);
    }

    void Update()
    {
        // Page turning animation logic
        for (int i = 0; i < totalPages; i++)
        {
            if (page >= i)
            {
                pageAngle[i] += Time.deltaTime * speed;
                if (pageAnim[i] != null)
                    pageAnim[i].GetComponent<Animation>().Play("RL");
            }
            else
            {
                pageAngle[i] -= Time.deltaTime * speed;
                if (pageAnim[i] != null)
                    pageAnim[i].GetComponent<Animation>().Play("LR");
            }
            pageAngle[i] = Mathf.Clamp(pageAngle[i], pageAngleMin[i], pageAngleMax[i]);
            pageLeaf[i].localEulerAngles = new Vector3(0.0f, pageAngle[i], 0.0f);
        }

        // Check player distance for showing/hiding interact button
        if (player != null && mainCamera.enabled)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist <= showButtonDistance)
            {
                if (!interactButton.activeSelf && !reactivateButton.activeSelf)
                    interactButton.SetActive(true);
            }
            else
            {
                if (interactButton.activeSelf)
                    interactButton.SetActive(false);
            }
        }
    }

    public void TurnPage(int direction)
    {
        switch (direction)
        {
            case -1:
                if (page < totalPages - 1)
                {
                    page++;
                    if (page == 0 || page == totalPages - 1)
                        myAudio.PlayOneShot(openCloseSound);
                    else
                        myAudio.PlayOneShot(pageTurnSound);
                }
                break;
            case 1:
                if (page > -1)
                {
                    page--;
                    if (page == -1 || page == totalPages - 2)
                        myAudio.PlayOneShot(openCloseSound);
                    else
                        myAudio.PlayOneShot(pageTurnSound);
                }
                break;
        }
    }

    void TurnToPage(int num)
    {
        page = num;
        myAudio.PlayOneShot(pageTurnSound);
    }

    // Called when Interact Button clicked
    public void OnInteractButtonClicked()
    {
        mainCamera.enabled = false;
        interactButton.SetActive(false);
        reactivateButton.SetActive(true);
    }

    // Called when Reactivate Button clicked
    public void OnReactivateButtonClicked()
    {
        mainCamera.enabled = true;
        reactivateButton.SetActive(false);
        interactButton.SetActive(true);
    }
}
