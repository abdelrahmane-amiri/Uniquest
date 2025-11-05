using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/savegame.json";
    
    public static void SaveGame()
    {
        SaveData data = new SaveData();
        
        // Sauvegarder les stats depuis GameManager
        if (GameManager.instance != null)
        {
            data.playerClass = GameManager.instance.playerClass;
            data.level = GameManager.instance.level;
            data.currentXP = GameManager.instance.currentXP;
            data.xpToNextLevel = GameManager.instance.xpToNextLevel;
            
            data.currentHealth = GameManager.instance.currentHealth;
            data.maxHealth = GameManager.instance.maxHealth;
            data.currentMana = GameManager.instance.currentMana;
            data.maxMana = GameManager.instance.maxMana;
            
            data.strength = GameManager.instance.strength;
            data.agility = GameManager.instance.agility;
            data.intelligence = GameManager.instance.intelligence;
            data.defense = GameManager.instance.defense;
        }
        
        // Sauvegarder l'inventaire
        data.inventory = new System.Collections.Generic.List<ItemData>();
        if (Inventory.instance != null)
        {
            foreach (Item item in Inventory.instance.items)
            {
                data.inventory.Add(new ItemData(item));
            }
        }
        
        // Sauvegarder la position du joueur
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            data.posX = player.transform.position.x;
            data.posY = player.transform.position.y;
            data.posZ = player.transform.position.z;
        }
        
        // Sauvegarder la scène actuelle
        data.currentScene = SceneManager.GetActiveScene().name;
        
        // Convertir en JSON et sauvegarder
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        
        Debug.Log($"💾 Partie sauvegardée dans : {savePath}");
    }
    
    public static SaveData LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("❌ Aucune sauvegarde trouvée !");
            return null;
        }
        
        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        
        Debug.Log($"📂 Partie chargée : Niveau {data.level}, {data.playerClass}");
        return data;
    }
    
    public static void ApplyLoadedData(SaveData data)
    {
        if (data == null) return;
        
        // Appliquer au GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.playerClass = data.playerClass;
            GameManager.instance.level = data.level;
            GameManager.instance.currentXP = data.currentXP;
            GameManager.instance.xpToNextLevel = data.xpToNextLevel;
            
            GameManager.instance.currentHealth = data.currentHealth;
            GameManager.instance.maxHealth = data.maxHealth;
            GameManager.instance.currentMana = data.currentMana;
            GameManager.instance.maxMana = data.maxMana;
            
            GameManager.instance.strength = data.strength;
            GameManager.instance.agility = data.agility;
            GameManager.instance.intelligence = data.intelligence;
            GameManager.instance.defense = data.defense;
        }
        
        // Restaurer l'inventaire
        if (Inventory.instance != null)
        {
            Inventory.instance.items.Clear();
            foreach (ItemData itemData in data.inventory)
            {
                Inventory.instance.items.Add(itemData.ToItem());
            }
            Debug.Log($"🎒 {data.inventory.Count} items restaurés dans l'inventaire");
        }
        
        // Charger la scène sauvegardée
        if (!string.IsNullOrEmpty(data.currentScene))
        {
            SceneManager.LoadScene(data.currentScene);
        }
    }
    
    public static bool HasSaveFile()
    {
        return File.Exists(savePath);
    }
    
    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Sauvegarde supprimée");
        }
    }
}
