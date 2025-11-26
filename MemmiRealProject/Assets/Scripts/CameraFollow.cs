using UnityEngine;

public class CameraFollowWithBounds : MonoBehaviour
{
    [Header("Target Player")]
    public Transform target;       // Player ที่กล้องตาม
    public Vector3 offset = new Vector3(0, 0, -10); // ระยะกล้องจาก Player
    public float smoothSpeed = 0.125f;

    [Header("Background")]
    public SpriteRenderer bgSprite; // BG Sprite เพื่อคำนวณ Bounds

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

        // ขนาดกล้องครึ่งหน้าจอ
        Camera cam = GetComponent<Camera>();
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        // คำนวณ Bounds จาก BG Sprite
        Vector3 bgPos = bgSprite.transform.position;
        Vector3 bgSize = bgSprite.bounds.size; // ขนาดจริงใน World Units

        minBounds = new Vector2(bgPos.x - bgSize.x / 2f, bgPos.y - bgSize.y / 2f);
        maxBounds = new Vector2(bgPos.x + bgSize.x / 2f, bgPos.y + bgSize.y / 2f);
    }

    void LateUpdate()
    {
        if (target == null || bgSprite == null) return;

        // ตำแหน่งเป้าหมาย + Offset
        Vector3 desiredPos = target.position + offset;

        // จำกัดกล้องให้อยู่ใน Bounds
        float clampedX = Mathf.Clamp(desiredPos.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPos.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

        Vector3 clampedPos = new Vector3(clampedX, clampedY, desiredPos.z);

        // Smooth Follow
        transform.position = Vector3.Lerp(transform.position, clampedPos, smoothSpeed);
    }
}
