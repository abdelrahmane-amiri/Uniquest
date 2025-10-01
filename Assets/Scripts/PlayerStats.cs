using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
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
    public String PlayerClass;

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
    public int level = 1;

    [Header("Statistiques classe")]
    public ClassStats archerStats = new ClassStats { health = 100, mana = 60, strength = 6, agility = 9, intelligence = 5, defense = 4 };
    public ClassStats moineStats = new ClassStats { health = 100, mana = 120, strength = 4, agility = 5, intelligence = 9, defense = 6 };
    public ClassStats warriorStats = new ClassStats { health = 100, mana = 40, strength = 9, agility = 4, intelligence = 3, defense = 8 };

    [Header("Références")]
    [SerializeField] Dialogue dialogue;


    void Start()
    {
        archerButton.onClick.AddListener(ArcherAction);
        moineButton.onClick.AddListener(MoineAction);
        warriorButton.onClick.AddListener(WarriorAction);
    }

    // Update is called once per frame
    void Update()
    {
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
        maxMana = stats.mana;
        strength = stats.strength;
        agility = stats.agility;
        intelligence = stats.intelligence;
        defense = stats.defense;
    }

    void ArcherAction()
    {
        Debug.Log("Le boutton Archer activer");
        PlayerClass = "Archer";
        ApplyStats(archerStats);
        dialogue.endDialog = true;
        dialogue.selectClass = false;
        archerButton.gameObject.SetActive(false);
        warriorButton.gameObject.SetActive(false);
        moineButton.gameObject.SetActive(false);
    }

    void MoineAction()
    {
        Debug.Log("Le boutton Moine activer");
        PlayerClass = "Moine";
        ApplyStats(moineStats);
        dialogue.endDialog = true;
        dialogue.selectClass = false;
        archerButton.gameObject.SetActive(false);
        warriorButton.gameObject.SetActive(false);
        moineButton.gameObject.SetActive(false);
    }

    void WarriorAction()
    {
        Debug.Log("Le boutton Warrior activer");
        PlayerClass = "Warrior";
        ApplyStats(warriorStats);
        dialogue.endDialog = true;
        dialogue.selectClass = false;
        archerButton.gameObject.SetActive(false);
        warriorButton.gameObject.SetActive(false);
        moineButton.gameObject.SetActive(false);
    }
}
