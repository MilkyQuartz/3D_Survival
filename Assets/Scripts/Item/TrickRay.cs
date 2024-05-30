using UnityEngine;

public class TrickRay : MonoBehaviour, IInteractable
{
    public TrickItemData trickItemData;
    public Transform[] laserStartPoints; 
    private Transform[] laserDirections; 
    public float shootInterval;
    public LayerMask groundLayerMask; 
    private float maxLaserLength; 
    private float lastShootTime; 

    void Start()
    {
        laserDirections = new Transform[laserStartPoints.Length];
        for (int i = 0; i < laserDirections.Length; i++)
        {
            laserDirections[i] = transform; 
        }
        maxLaserLength = CalculateMaxLaserLength();
    }

    void Update()
    {
        if (Time.time - lastShootTime >= shootInterval)
        {
            for (int i = 0; i < laserStartPoints.Length; i++)
            {
                ShootLaser(laserStartPoints[i], laserDirections[i]);
            }
            lastShootTime = Time.time;
        }
    }

    float CalculateMaxLaserLength()
    {
        RaycastHit hit;
        float maxDistance = 30f;
        foreach (var startPoint in laserStartPoints)
        {
            if (Physics.Raycast(startPoint.position, Vector3.down, out hit, maxDistance, groundLayerMask))
            {
                if (hit.distance < maxDistance)
                {
                    maxDistance = hit.distance;
                }
            }
        }
        return maxDistance;
    }

    void ShootLaser(Transform startPoint, Transform direction)
    {
        Vector3 startPos = startPoint.position;
        Vector3 directionVector = Vector3.down;

        RaycastHit hit;
        if (Physics.Raycast(startPos, directionVector, out hit, maxLaserLength))
        {
            Debug.DrawRay(startPos, directionVector * hit.distance, Color.red, 2f);
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("레이저 구간 지나감");
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