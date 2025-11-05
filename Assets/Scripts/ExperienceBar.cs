using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("XP et Niveau")]
    public Slider xpSlider;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI levelText;

    [Header("Stats Vitales")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;

    [Header("Attributs")]
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI intelligenceText;
    public TextMeshProUGUI defenseText;

    [Header("Classe (optionnel)")]
    public TextMeshProUGUI classText;

    [Header("Animation")]
    public float fillSpeed = 5f;
    
    private float targetFillAmount = 0f;
    private float currentFillAmount = 0f;

    void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("❌ GameManager introuvable !");
            return;
        }

        UpdateAllStats();
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        // Anime la barre d'XP
        if (Mathf.Abs(currentFillAmount - targetFillAmount) > 0.001f)
        {
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * fillSpeed);
            
            if (xpSlider != null)
            {
                xpSlider.value = currentFillAmount;
            }
        }

        // Met à jour toutes les stats
        UpdateAllStats();
    }

    void UpdateAllStats()
    {
        if (GameManager.instance == null) return;

        // XP et Niveau
        targetFillAmount = (float)GameManager.instance.currentXP / GameManager.instance.xpToNextLevel;
        
        if (xpText != null)
            xpText.text = $"XP: {GameManager.instance.currentXP} / {GameManager.instance.xpToNextLevel}";
        
        if (levelText != null)
            levelText.text = $"Niveau {GameManager.instance.level}";

        // Stats Vitales
        if (healthText != null)
            healthText.text = $"PV: {GameManager.instance.currentHealth} / {GameManager.instance.maxHealth}";
        
        if (manaText != null)
            manaText.text = $"Mana: {GameManager.instance.currentMana} / {GameManager.instance.maxMana}";

        // Attributs
        if (strengthText != null)
            strengthText.text = $"Force: {GameManager.instance.strength}";
        
        if (agilityText != null)
            agilityText.text = $"Agilité: {GameManager.instance.agility}";
        
        if (intelligenceText != null)
            intelligenceText.text = $"Intelligence: {GameManager.instance.intelligence}";
        
        if (defenseText != null)
            defenseText.text = $"Défense: {GameManager.instance.defense}";

        // Classe (si tu veux l'afficher)
        if (classText != null && !string.IsNullOrEmpty(GameManager.instance.playerClass))
            classText.text = $"Classe: {GameManager.instance.playerClass}";
    }
}