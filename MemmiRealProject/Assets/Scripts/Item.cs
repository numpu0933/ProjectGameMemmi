using UnityEngine;

public class Item : MonoBehaviour
{
    public int coinValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.coins += coinValue;

                if (AudioManager.Instance != null && AudioManager.Instance.coinSound != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.coinSound);
                }
            }

            Destroy(gameObject);
        }
    }
}
