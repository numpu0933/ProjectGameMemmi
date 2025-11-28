using UnityEngine;

public class SimpleSpriteChange : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite walkSprite;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            sr.sprite = walkSprite;
        }
        else
        {
            sr.sprite = idleSprite;
        }
    }
}
