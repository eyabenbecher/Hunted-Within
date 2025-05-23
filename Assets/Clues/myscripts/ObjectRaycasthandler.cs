using UnityEngine;

public class ObjectRaycastHandler : MonoBehaviour
{
    [SerializeField] private GameObject canvasUI; 
    [SerializeField] private GameObject targetObject;

    void Start()
    {
        if (canvasUI != null)
        {
            canvasUI.SetActive(false); 
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Raycast hit: {hit.collider.name}"); 

            
                if (hit.collider.gameObject == targetObject)
                {
                    Debug.Log("Target object clicked!");
                    if (canvasUI != null)
                    {
                        canvasUI.SetActive(true); 
                    }
                }
            }
        }

      
       
    }
    public void Quit()
    {
        
            canvasUI.SetActive(false);
        
    }
}
