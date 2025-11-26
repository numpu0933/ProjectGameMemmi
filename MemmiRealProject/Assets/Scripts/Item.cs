using UnityEngine;

public class Item : MonoBehaviour
{
    public int coinValue = 1; // จำนวนเหรียญที่จะเพิ่ม

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().coins += coinValue;
            Destroy(gameObject); // ทำลายไอเท็มหลังเก็บ
        }
    }
}
