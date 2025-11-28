using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; 

    [Header("Player Stats UI")]
    public Slider healthBar;
    public TextMeshProUGUI coinText; 
    public TextMeshProUGUI gameOverText;

    public PlayerStats playerStats;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (playerStats != null && healthBar != null)
        {
            healthBar.maxValue = playerStats.maxHealth;
            healthBar.value = playerStats.currentHealth;
        }

        if (gameOverText != null)
            gameOverText.alpha = 0; 
    }

    void Update()
    {
        if (playerStats == null) return;

        
        if (healthBar != null)
            healthBar.value = playerStats.currentHealth;

        
        if (coinText != null)
            coinText.text = "Coins: " + playerStats.coins;

        
        if (playerStats.currentHealth <= 0 && gameOverText != null)
        {
            gameOverText.alpha = 1;
        }
    }
}
