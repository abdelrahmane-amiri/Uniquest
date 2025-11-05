using UnityEngine;

[System.Serializable]
public enum ItemType
{
    HealthPotion,
    ManaPotion,
    StrengthBoost,
    Key
}

[System.Serializable]
public class Item
{
    public string itemName;
    public ItemType type;
    public int value;
    public string description;
    
    public Item(string name, ItemType itemType, int val, string desc)
    {
        itemName = name;
        type = itemType;
        value = val;
        description = desc;
    }
    
    public void Use(PlayerStats player)
    {
        switch (type)
        {
            case ItemType.HealthPotion:
                int healAmount = value;
                player.currentHealth = Mathf.Min(player.maxHealth, player.currentHealth + healAmount);
                if (GameManager.instance != null)
                    GameManager.instance.currentHealth = player.currentHealth;
                Debug.Log($"🧪 {itemName} utilisé ! +{healAmount} PV");
                break;
                
            case ItemType.ManaPotion:
                int manaAmount = value;
                player.currentMana = Mathf.Min(player.maxMana, player.currentMana + manaAmount);
                if (GameManager.instance != null)
                    GameManager.instance.currentMana = player.currentMana;
                Debug.Log($"🔵 {itemName} utilisé ! +{manaAmount} mana");
                break;
                
            case ItemType.StrengthBoost:
                player.strength += value;
                if (GameManager.instance != null)
                    GameManager.instance.strength = player.strength;
                Debug.Log($"💪 {itemName} utilisé ! +{value} force");
                break;
                
            case ItemType.Key:
                Debug.Log($"🔑 {itemName} - Clé spéciale !");
                break;
        }
    }
}
