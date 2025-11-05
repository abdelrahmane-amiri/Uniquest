using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform itemListParent;
    public GameObject itemButtonPrefab;
    public TextMeshProUGUI titleText;
    
    private bool isOpen = false;
    private List<GameObject> itemButtons = new List<GameObject>();
    
    void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }
    
    void Update()
    {
        // Ouvrir/Fermer avec 'B' (Bag) ou 'Tab'
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
        
        // Fermer avec Echap
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseInventory();
        }
    }
    
    public void ToggleInventory()
    {
        if (isOpen)
            CloseInventory();
        else
            OpenInventory();
    }
    
    public void OpenInventory()
    {
        if (inventoryPanel == null) return;
        
        isOpen = true;
        inventoryPanel.SetActive(true);
        Time.timeScale = 0f; // Pause le jeu
        
        RefreshInventoryDisplay();
        
        Debug.Log("🎒 Inventaire ouvert");
    }
    
    public void CloseInventory()
    {
        if (inventoryPanel == null) return;
        
        isOpen = false;
        inventoryPanel.SetActive(false);
        Time.timeScale = 1f; // Reprend le jeu
        
        Debug.Log("🎒 Inventaire fermé");
    }
    
    void RefreshInventoryDisplay()
    {
        // Nettoyer les anciens boutons
        foreach (GameObject btn in itemButtons)
        {
            Destroy(btn);
        }
        itemButtons.Clear();
        
        if (Inventory.instance == null) return;
        
        // Compter les items par type
        Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>();
        
        foreach (Item item in Inventory.instance.items)
        {
            if (itemCounts.ContainsKey(item.type))
                itemCounts[item.type]++;
            else
                itemCounts[item.type] = 1;
        }
        
        // Créer un bouton par type d'item
        foreach (var kvp in itemCounts)
        {
            ItemType type = kvp.Key;
            int count = kvp.Value;
            
            CreateItemButton(type, count);
        }
        
        // Afficher le total
        if (titleText != null)
            titleText.text = $"INVENTAIRE ({Inventory.instance.items.Count}/{Inventory.instance.maxItems})";
    }
    
    void CreateItemButton(ItemType type, int count)
    {
        if (itemListParent == null) return;
        
        // Créer un panel pour chaque item (fond coloré)
        GameObject itemPanel = new GameObject($"Item_{type}");
        itemPanel.transform.SetParent(itemListParent, false);
        
        // RectTransform
        RectTransform panelRect = itemPanel.AddComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(420, 50);
        
        // Image de fond
        Image bgImage = itemPanel.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.3f, 0.8f); // Gris-bleu foncé
        
        // Ajouter un outline pour le style
        Outline outline = itemPanel.AddComponent<Outline>();
        outline.effectColor = new Color(0.5f, 0.5f, 0.6f, 1f);
        outline.effectDistance = new Vector2(2, -2);
        
        // Créer le texte enfant
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(itemPanel.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0);
        textRect.anchorMax = new Vector2(1, 1);
        textRect.offsetMin = new Vector2(15, 5); // Padding gauche et bas
        textRect.offsetMax = new Vector2(-15, -5); // Padding droit et haut
        
        // Text
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.fontSize = 26;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Left;
        text.fontStyle = FontStyles.Bold;
        
        string itemName = GetItemName(type);
        string icon = GetItemIcon(type);
        text.text = $"{icon}  {itemName}  <color=#FFD700>x{count}</color>"; // Quantité en or
        
        // Ajouter Button sur le panel
        Button button = itemPanel.AddComponent<Button>();
        
        // Changer couleur au hover
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.2f, 0.2f, 0.3f, 0.8f);
        colors.highlightedColor = new Color(0.3f, 0.3f, 0.5f, 1f);
        colors.pressedColor = new Color(0.15f, 0.15f, 0.25f, 1f);
        button.colors = colors;
        
        // Action au clic
        button.onClick.AddListener(() => UseItemFromInventory(type));
        
        itemButtons.Add(itemPanel);
    }
    
    string GetItemName(ItemType type)
    {
        switch (type)
        {
            case ItemType.HealthPotion: return "Potion de soin";
            case ItemType.ManaPotion: return "Potion de mana";
            case ItemType.StrengthBoost: return "Potion de force";
            case ItemType.Key: return "Clé";
            default: return "Item";
        }
    }
    
    string GetItemIcon(ItemType type)
    {
        switch (type)
        {
            case ItemType.HealthPotion: return "🧪";
            case ItemType.ManaPotion: return "🔵";
            case ItemType.StrengthBoost: return "💪";
            case ItemType.Key: return "🔑";
            default: return "📦";
        }
    }
    
    void UseItemFromInventory(ItemType type)
    {
        if (Inventory.instance == null) return;
        
        // Trouver le joueur
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player == null)
        {
            Debug.Log("❌ Joueur introuvable !");
            return;
        }
        
        // Récupérer l'item
        Item item = Inventory.instance.GetFirstItemOfType(type);
        if (item == null)
        {
            Debug.Log("❌ Plus d'item de ce type !");
            return;
        }
        
        // Vérifier si on peut l'utiliser
        if (type == ItemType.HealthPotion && player.currentHealth >= player.maxHealth)
        {
            Debug.Log("❌ PV déjà au maximum !");
            return;
        }
        
        if (type == ItemType.ManaPotion && player.currentMana >= player.maxMana)
        {
            Debug.Log("❌ Mana déjà au maximum !");
            return;
        }
        
        // Utiliser l'item
        item.Use(player);
        
        // Retirer de l'inventaire
        Inventory.instance.RemoveItem(item);
        
        // Rafraîchir l'affichage
        RefreshInventoryDisplay();
    }
}
