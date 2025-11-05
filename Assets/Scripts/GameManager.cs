using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Player stats persistantes
    public string playerClass;
    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;
    public int strength;
    public int agility;
    public int intelligence;
    public int defense;

    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ne détruit pas ce GameObject entre les scènes
        }
        else
        {
            Destroy(gameObject); // empêche les doublons
        }
    }
}
