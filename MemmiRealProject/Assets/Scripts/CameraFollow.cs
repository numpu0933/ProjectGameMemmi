using UnityEngine;

public class CameraFollowWithBounds : MonoBehaviour
{
    [Header("Target Player")]
    public Transform target;       
    public Vector3 offset = new Vector3(0, 0, -10); 
    public float smoothSpeed = 0.125f;

    [Header("Background")]
    public SpriteRenderer bgSprite; 

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    void Start()
    {
        if (bgSprite == null)
        {
            Debug.LogError("BG Sprite ยังไม่ได้เชื่อม!");
            return;
        }

        
        Camera cam = GetComponent<Camera>();
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

       
        Vector3 bgPos = bgSprite.transform.position;
        Vector3 bgSize = bgSprite.bounds.size; 

        minBounds = new Vector2(bgPos.x - bgSize.x / 2f, bgPos.y - bgSize.y / 2f);
        maxBounds = new Vector2(bgPos.x + bgSize.x / 2f, bgPos.y + bgSize.y / 2f);
    }

    void LateUpdate()
    {
        if (target == null || bgSprite == null) return;

        
        Vector3 desiredPos = target.position + offset;

        
        float clampedX = Mathf.Clamp(desiredPos.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPos.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

        Vector3 clampedPos = new Vector3(clampedX, clampedY, desiredPos.z);

        
        transform.position = Vector3.Lerp(transform.position, clampedPos, smoothSpeed);
    }
}
