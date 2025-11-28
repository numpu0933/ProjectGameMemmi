using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public Transform targetPosition;      
    public float targetSize = 5f;         
    public float transitionSpeed = 2f;    
    private bool playerInside = false;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogWarning("Main Camera not found!");
    }

    void Update()
    {
        if (mainCamera == null) return;

        if (playerInside)
        {
            // เลื่อนกล้องไปยัง target
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                new Vector3(targetPosition.position.x, targetPosition.position.y, mainCamera.transform.position.z),
                Time.deltaTime * transitionSpeed
            );

            // ซูมกล้อง
            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize,
                targetSize,
                Time.deltaTime * transitionSpeed
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
