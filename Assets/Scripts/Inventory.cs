using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    
    public List<Item> items = new List<Item>();
    public int maxItems = 20;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeStartingItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeStartingItems()
    {
        // Ajouter des items de départ
        AddItem(new Item("Potion de soin", ItemType.HealthPotion, 50, "Restaure 50 PV"));
        AddItem(new Item("Potion de soin", ItemType.HealthPotion, 50, "Restaure 50 PV"));
        AddItem(new Item("Potion de soin", ItemType.HealthPotion, 50, "Restaure 50 PV"));
        
        AddItem(new Item("Potion de mana", ItemType.ManaPotion, 30, "Restaure 30 mana"));
        AddItem(new Item("Potion de mana", ItemType.ManaPotion, 30, "Restaure 30 mana"));
        
        AddItem(new Item("Potion de force", ItemType.StrengthBoost, 5, "+5 force"));
        
        Debug.Log($"🎒 Inventaire initialisé avec {items.Count} items");
    }
    
    public bool AddItem(Item item)
    {
        if (items.Count >= maxItems)
        {
            Debug.Log("❌ Inventaire plein !");
            return false;
        }
        
        items.Add(item);
        Debug.Log($"✅ {item.itemName} ajouté à l'inventaire");
        return true;
    }
    
    public bool RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"🗑️ {item.itemName} retiré de l'inventaire");
            return true;
        }
        return false;
    }
    
    public bool HasItem(ItemType type)
    {
        foreach (Item item in items)
        {
            if (item.type == type)
                return true;
        }
        return false;
    }
    
    public Item GetFirstItemOfType(ItemType type)
    {
        foreach (Item item in items)
        {
            if (item.type == type)
                return item;
        }
        return null;
    }
    
    public int CountItemsOfType(ItemType type)
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item.type == type)
                count++;
        }
        return count;
    }
}
