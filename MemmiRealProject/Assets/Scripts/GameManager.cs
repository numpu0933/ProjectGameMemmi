using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject winPanel;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void WinGame()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}
