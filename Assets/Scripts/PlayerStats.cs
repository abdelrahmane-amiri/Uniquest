using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ClassStats
{
    public int health;
    public int mana;
    public int strength;
    public int agility;
    public int intelligence;
    public int defense;
}

public class PlayerStats : MonoBehaviour
{
    [Header("Classe du joueur")]
    public string PlayerClass;

    [Header("Animators de classe")]
    public AnimatorOverrideController archerClass;
    public AnimatorOverrideController moineClass;
    public AnimatorOverrideController warriorClass;

    [Header("Boutons de sélection")]
    public Button archerButton;
    public Button moineButton;
    public Button warriorButton;

    [Header("Statistiques actuelles")]
    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;
    public int strength;
    public int agility;
    public int intelligence;
    public int defense;

    [Header("Expérience")]
    public int level = 1;     
    public int currentXP = 0; 
    public int xpToNextLevel = 100;

    [Header("Attaques")]
    public Attack[] physicalAttacks;
    public Attack[] magicAttacks;

    [Header("Statistiques classe")]
    public ClassStats archerStats = new ClassStats { health = 100, mana = 60, strength = 6, agility = 9, intelligence = 5, defense = 4 };
    public ClassStats moineStats = new ClassStats { health = 100, mana = 120, strength = 4, agility = 5, intelligence = 9, defense = 6 };
    public ClassStats warriorStats = new ClassStats { health = 100, mana = 40, strength = 9, agility = 4, intelligence = 3, defense = 8 };

    [Header("Références")]
    [SerializeField] Dialogue dialogue;

    void Start()
    {
        // Boutons pour la sélection de classe
        archerButton.onClick.AddListener(ArcherAction);
        moineButton.onClick.AddListener(MoineAction);
        warriorButton.onClick.AddListener(WarriorAction);

        // Si GameManager existe, récupère les stats du joueur
        if (GameManager.instance != null)
        {
            currentHealth = GameManager.instance.currentHealth;
            maxHealth = GameManager.instance.maxHealth;
            currentMana = GameManager.instance.currentMana;
            maxMana = GameManager.instance.maxMana;
            strength = GameManager.instance.strength;
            agility = GameManager.instance.agility;
            intelligence = GameManager.instance.intelligence;
            defense = GameManager.instance.defense;
            PlayerClass = GameManager.instance.playerClass;
        }
        
        // Initialiser les attaques APRÈS avoir chargé les stats
        InitializeAttacks();
    }

    void Update()
    {
        // Affiche les boutons si on doit sélectionner une classe
        if (dialogue.selectClass)
        {
            archerButton.gameObject.SetActive(true);
            warriorButton.gameObject.SetActive(true);
            moineButton.gameObject.SetActive(true);
        }
    }

    void ApplyStats(ClassStats stats)
    {
        maxHealth = stats.health;
        currentHealth = stats.health;
        maxMana = stats.mana;
        currentMana = stats.mana;
        strength = stats.strength;
        agility = stats.agility;
        intelligence = stats.intelligence;
        defense = stats.defense;

        // 🔹 Met à jour le GameManager pour persistance
        if (GameManager.instance != null)
        {
            GameManager.instance.currentHealth = currentHealth;
            GameManager.instance.maxHealth = maxHealth;
            GameManager.instance.currentMana = currentMana;
            GameManager.instance.maxMana = maxMana;
            GameManager.instance.strength = strength;
            GameManager.instance.agility = agility;
            GameManager.instance.intelligence = intelligence;
            GameManager.instance.defense = defense;
            GameManager.instance.playerClass = PlayerClass;
        }
        
        // Réinitialiser les attaques avec les nouvelles stats
        InitializeAttacks();
    }

    void ArcherAction()
    {
        Debug.Log("Le bouton Archer activé");
        PlayerClass = "Archer";
        ApplyStats(archerStats);
        dialogue.endDialog = true;
        dialogue.selectClass = false;
        HideButtons();
    }

    void MoineAction()
    {
        Debug.Log("Le bouton Moine activé");
        PlayerClass = "Moine";
        ApplyStats(moineStats);
        dialogue.endDialog = true;
        dialogue.selectClass = false;
        HideButtons();
    }

    void WarriorAction()
    {
        Debug.Log("Le bouton Warrior activé");
        PlayerClass = "Warrior";
        ApplyStats(warriorStats);
        dialogue.endDialog = true;
        dialogue.selectClass = false;
        HideButtons();
    }

    void HideButtons()
    {
        archerButton.gameObject.SetActive(false);
        warriorButton.gameObject.SetActive(false);
        moineButton.gameObject.SetActive(false);
    }

    // 🔹 Méthode pour infliger des dégâts au joueur
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth < 0) currentHealth = 0;

        // Met à jour le GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.currentHealth = currentHealth;
        }
    }

    public void GainXP(int amount)
    {
        if (GameManager.instance == null) return;

        // Travaille directement sur le GameManager
        GameManager.instance.currentXP += amount;
        Debug.Log($"🟢 Gagné {amount} XP ! Total: {GameManager.instance.currentXP}/{GameManager.instance.xpToNextLevel}");

        // Vérifie le level up
        while (GameManager.instance.currentXP >= GameManager.instance.xpToNextLevel)
        {
            LevelUpGameManager();
        }
        
        // Synchronise les stats locales
        currentXP = GameManager.instance.currentXP;
        level = GameManager.instance.level;
        xpToNextLevel = GameManager.instance.xpToNextLevel;
    }

    private void LevelUpGameManager()
    {
        if (GameManager.instance == null) return;

        GameManager.instance.currentXP -= GameManager.instance.xpToNextLevel;
        GameManager.instance.level++;
        GameManager.instance.xpToNextLevel = Mathf.RoundToInt(GameManager.instance.xpToNextLevel * 1.3f);

        Debug.Log($"🎉 Niveau {GameManager.instance.level} atteint !");

        // Bonus de stats sur le GameManager
        GameManager.instance.maxHealth += 10;
        GameManager.instance.strength += 2;
        GameManager.instance.agility += 1;
        GameManager.instance.defense += 1;
        GameManager.instance.currentHealth = GameManager.instance.maxHealth;
        
        // Synchronise les stats locales
        maxHealth = GameManager.instance.maxHealth;
        currentHealth = GameManager.instance.currentHealth;
        strength = GameManager.instance.strength;
        agility = GameManager.instance.agility;
        defense = GameManager.instance.defense;
    }

    void InitializeAttacks()
    {
        Debug.Log($"🔧 Initialisation attaques - Intelligence: {intelligence}, Force: {strength}");
        
        // Attaques physiques
        physicalAttacks = new Attack[]
        {
            new Attack("Coup d'épée", AttackType.Physical, strength * 2, 0, 95, 10),
            new Attack("Coup puissant", AttackType.Physical, strength * 3, 0, 80, 15)
        };

        // Attaques magiques (basées sur intelligence)
        magicAttacks = new Attack[]
        {
            new Attack("Boule de feu", AttackType.Magic, intelligence * 3, 20, 90, 5),
            new Attack("Éclair", AttackType.Magic, intelligence * 4, 30, 85, 10),
            new Attack("Soin", AttackType.Magic, intelligence * 2, 15, 100, 0)
        };
        
        Debug.Log($"✅ Attaques créées - Boule de feu: {magicAttacks[0].damage} dégâts");
    }

    public void InitializeAttacksPublic()
    {
        InitializeAttacks();
    }

}
