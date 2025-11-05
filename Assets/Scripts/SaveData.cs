using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Stats du joueur
    public string playerClass;
    public int level;
    public int currentXP;
    public int xpToNextLevel;
    
    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;
    
    public int strength;
    public int agility;
    public int intelligence;
    public int defense;
    
    // Inventaire
    public List<ItemData> inventory;
    
    // Position du joueur
    public float posX;
    public float posY;
    public float posZ;
    
    // Scene
    public string currentScene;
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public string itemType;
    public int value;
    public string description;
    
    public ItemData(Item item)
    {
        itemName = item.itemName;
        itemType = item.type.ToString();
        value = item.value;
        description = item.description;
    }
    
    public Item ToItem()
    {
        ItemType type = (ItemType)Enum.Parse(typeof(ItemType), itemType);
        return new Item(itemName, type, value, description);
    }
}
