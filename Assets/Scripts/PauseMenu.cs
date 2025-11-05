using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button saveButton;
    public Button loadButton;
    public Button resumeButton;
    public Button quitButton;
    
    private bool isPaused = false;
    
    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        
        if (saveButton != null)
            saveButton.onClick.AddListener(SaveGame);
        
        if (loadButton != null)
            loadButton.onClick.AddListener(LoadGame);
        
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitToMenu);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Pause le jeu
        Debug.Log("⏸️ Jeu en pause");
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Reprend le jeu
        Debug.Log("▶️ Jeu repris");
    }
    
    public void SaveGame()
    {
        SaveSystem.SaveGame();
        Debug.Log("💾 Partie sauvegardée !");
    }
    
    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data != null)
        {
            Time.timeScale = 1f; // Reprend le temps avant de charger
            SaveSystem.ApplyLoadedData(data);
            Debug.Log("📂 Partie chargée !");
        }
        else
        {
            Debug.Log("❌ Aucune sauvegarde trouvée !");
        }
    }
    
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Change selon ton menu principal
        Debug.Log("🚪 Retour au menu");
    }
}
