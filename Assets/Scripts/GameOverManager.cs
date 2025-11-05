using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public Button quitButton;
    
    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitToMenu);
    }
    
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            
            if (gameOverText != null)
                gameOverText.text = "GAME OVER\n\nTu as été vaincu...";
            
            Debug.Log("💀 Game Over");
        }
    }
    
    void Retry()
    {
        Time.timeScale = 1f;
        
        // Reset GameManager stats
        if (GameManager.instance != null)
        {
            GameManager.instance.currentHealth = GameManager.instance.maxHealth;
            GameManager.instance.currentMana = GameManager.instance.maxMana;
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void QuitToMenu()
    {
        Time.timeScale = 1f;
        
        // Reset GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.currentHealth = GameManager.instance.maxHealth;
        }
        
        SceneManager.LoadScene("SampleScene");
    }
}
