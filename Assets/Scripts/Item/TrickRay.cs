using UnityEngine;

public class TrickRay : MonoBehaviour, IInteractable
{
    public TrickItemData trickItemData;
    public Transform rayA;
    public Transform rayB;
    public LayerMask layerMask;

    private LineRenderer lineRenderer;

    private void Start()
    {
        if(trickItemData.trickItemType == TrickItemType.Ray)
        {
            SetupRay();
        }
    }

    private void SetupRay()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, rayA.position);
            lineRenderer.SetPosition(1, rayB.position);
        }
        float distance = Vector3.Distance(rayA.position, rayB.position);

        RaycastHit hit;
        if (Physics.Raycast(rayA.position, rayB.position - rayA.position, out hit, distance, layerMask))
        {
            if (hit.transform == rayB)
            {
                Debug.Log("rayA에서 rayB를 감지함");
            }
        }
    }

    public string GetInteractPrompt()
    {
        return $"{trickItemData.displayName}\n{trickItemData.description}";
    }

    public void OnInteract()
    {
    }
}
