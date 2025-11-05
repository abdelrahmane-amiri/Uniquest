using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    [Header("Références Player")]
    public PlayerStats player;
    public Transform playerStartPos;
    private Animator playerAnimator;

    [Header("Références Ennemi")]
    public Transform enemyTransform;
    public Transform enemyStartPos;
    private Enemy currentEnemy;

    [Header("UI")]
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI playerManaText;
    public TextMeshProUGUI enemyHPText;
    public Button attackButton;
    public Button magicButton;
    public Button itemButton;
    public GameObject magicMenuPanel;
    public GameObject itemMenuPanel;
    public Button fireBallButton;
    public Button lightningButton;
    public Button healButton;
    public Button healthPotionButton;
    public Button manaPotionButton;
    public Button strengthPotionButton;
    public GameOverManager gameOverManager;

    [Header("Attaque Settings")]
    public float attackDistance = 2.0f;
    public float attackSpeed = 5f;
    public float attackDuration = 0.5f;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player non assigné dans CombatManager !");
            return;
        }

        playerAnimator = player.GetComponent<Animator>();

        // Générer un ennemi aléatoire
        currentEnemy = Enemy.GetRandomEnemy();
        Debug.Log($"⚔️ Un {currentEnemy.enemyName} apparaît ! (PV: {currentEnemy.maxHP}, ATK: {currentEnemy.attack})");

        if (GameManager.instance != null)
        {
            player.currentHealth = GameManager.instance.currentHealth;
            player.maxHealth = GameManager.instance.maxHealth;
            player.currentMana = GameManager.instance.currentMana;
            player.maxMana = GameManager.instance.maxMana;
            player.strength = GameManager.instance.strength;
            player.defense = GameManager.instance.defense;
            player.agility = GameManager.instance.agility;
            player.intelligence = GameManager.instance.intelligence;
            
            // Forcer l'initialisation des attaques avec les bonnes stats
            player.InitializeAttacksPublic();
        }

        UpdateUI();
        attackButton.onClick.AddListener(() => StartCoroutine(PlayerAttackCoroutine()));
        
        // Boutons de magie
        if (magicButton != null)
            magicButton.onClick.AddListener(OpenMagicMenu);
        
        if (magicMenuPanel != null)
            magicMenuPanel.SetActive(false);
        
        if (fireBallButton != null)
            fireBallButton.onClick.AddListener(() => StartCoroutine(PlayerMagicAttackCoroutine(0)));
        
        if (lightningButton != null)
            lightningButton.onClick.AddListener(() => StartCoroutine(PlayerMagicAttackCoroutine(1)));
        
        if (healButton != null)
            healButton.onClick.AddListener(() => StartCoroutine(PlayerMagicAttackCoroutine(2)));
            
        // Boutons d'inventaire
        if (itemButton != null)
            itemButton.onClick.AddListener(OpenItemMenu);
        
        if (itemMenuPanel != null)
            itemMenuPanel.SetActive(false);
        
        if (healthPotionButton != null)
            healthPotionButton.onClick.AddListener(() => UseItem(ItemType.HealthPotion));
        
        if (manaPotionButton != null)
            manaPotionButton.onClick.AddListener(() => UseItem(ItemType.ManaPotion));
        
        if (strengthPotionButton != null)
            strengthPotionButton.onClick.AddListener(() => UseItem(ItemType.StrengthBoost));
            
        UpdateItemButtons();
    }

    void UpdateUI()
    {
        playerHPText.text = $"HP: {player.currentHealth}/{player.maxHealth}";
        if (playerManaText != null)
            playerManaText.text = $"Mana: {player.currentMana}/{player.maxMana}";
        
        if (currentEnemy != null)
            enemyHPText.text = $"{currentEnemy.enemyName} HP: {currentEnemy.currentHP}/{currentEnemy.maxHP}";
    }

    void OpenMagicMenu()
    {
        if (magicMenuPanel != null)
        {
            magicMenuPanel.SetActive(!magicMenuPanel.activeSelf);
            
            // Fermer le menu items si ouvert
            if (itemMenuPanel != null && magicMenuPanel.activeSelf)
                itemMenuPanel.SetActive(false);
        }
    }
    
    void OpenItemMenu()
    {
        if (itemMenuPanel != null)
        {
            itemMenuPanel.SetActive(!itemMenuPanel.activeSelf);
            
            // Fermer le menu magie si ouvert
            if (magicMenuPanel != null && itemMenuPanel.activeSelf)
                magicMenuPanel.SetActive(false);
                
            UpdateItemButtons();
        }
    }
    
    void UpdateItemButtons()
    {
        if (Inventory.instance == null) return;
        
        // Afficher le nombre de potions disponibles
        if (healthPotionButton != null)
        {
            int count = Inventory.instance.CountItemsOfType(ItemType.HealthPotion);
            var text = healthPotionButton.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = $"🧪 Potion soin ({count})";
            healthPotionButton.interactable = count > 0;
        }
        
        if (manaPotionButton != null)
        {
            int count = Inventory.instance.CountItemsOfType(ItemType.ManaPotion);
            var text = manaPotionButton.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = $"🔵 Potion mana ({count})";
            manaPotionButton.interactable = count > 0;
        }
        
        if (strengthPotionButton != null)
        {
            int count = Inventory.instance.CountItemsOfType(ItemType.StrengthBoost);
            var text = strengthPotionButton.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = $"💪 Potion force ({count})";
            strengthPotionButton.interactable = count > 0;
        }
    }
    
    void UseItem(ItemType itemType)
    {
        if (Inventory.instance == null)
        {
            Debug.LogError("❌ Inventory introuvable !");
            return;
        }
        
        Item item = Inventory.instance.GetFirstItemOfType(itemType);
        if (item == null)
        {
            Debug.Log("❌ Pas d'item de ce type !");
            return;
        }
        
        // Utiliser l'item
        item.Use(player);
        
        // Retirer de l'inventaire
        Inventory.instance.RemoveItem(item);
        
        // Fermer le menu
        if (itemMenuPanel != null)
            itemMenuPanel.SetActive(false);
        
        // Mettre à jour l'UI
        UpdateUI();
        UpdateItemButtons();
        
        // Tour de l'ennemi (pas besoin de StartCoroutine ici)
    }

    IEnumerator PlayerMagicAttackCoroutine(int spellIndex)
    {
        Debug.Log($"🎯 Tentative lancer sort {spellIndex}");
        
        if (spellIndex < 0 || spellIndex >= player.magicAttacks.Length)
        {
            Debug.LogError($"❌ Index invalide: {spellIndex}, magicAttacks.Length = {player.magicAttacks?.Length ?? 0}");
            yield break;
        }

        Attack spell = player.magicAttacks[spellIndex];
        Debug.Log($"✨ Sort sélectionné: {spell.attackName}, Coût: {spell.manaCost}, Mana actuel: {player.currentMana}");
        
        // Vérifier si assez de mana
        if (player.currentMana < spell.manaCost)
        {
            Debug.Log("Pas assez de mana !");
            yield break;
        }

        // Fermer le menu magie
        if (magicMenuPanel != null)
            magicMenuPanel.SetActive(false);

        attackButton.interactable = false;
        if (magicButton != null)
            magicButton.interactable = false;

        // Consommer le mana
        player.currentMana -= spell.manaCost;
        if (GameManager.instance != null)
            GameManager.instance.currentMana = player.currentMana;

        // Si c'est un sort de soin
        if (spell.attackName == "Soin")
        {
            int healAmount = spell.damage;
            player.currentHealth = Mathf.Min(player.maxHealth, player.currentHealth + healAmount);
            
            if (GameManager.instance != null)
                GameManager.instance.currentHealth = player.currentHealth;
            
            Debug.Log($"💚 Soin de {healAmount} PV !");
            UpdateUI();
            
            yield return new WaitForSeconds(1f);
        }
        else
        {
            // Attaque magique offensive
            Vector3 startPos = playerStartPos.position;
            
            playerAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackDuration);

            int damageToEnemy = spell.damage;
            currentEnemy.currentHP -= damageToEnemy;
            Debug.Log($"✨ {spell.attackName} inflige {damageToEnemy} dégâts !");
            UpdateUI();
        }

        if (currentEnemy.currentHP <= 0)
        {
            player.GainXP(currentEnemy.xpReward);
            EndCombat();
            yield break;
        }

        yield return StartCoroutine(EnemyAttackCoroutine());

        attackButton.interactable = true;
        if (magicButton != null)
            magicButton.interactable = true;

        if (player.currentHealth <= 0)
        {
            if (gameOverManager != null)
                gameOverManager.ShowGameOver();
            else
                EndCombat();
        }
    }

    IEnumerator PlayerAttackCoroutine()
    {
        attackButton.interactable = false;
        if (magicButton != null)
            magicButton.interactable = false;

        Vector3 startPos = playerStartPos.position;
        Vector3 direction = (enemyTransform.position - startPos).normalized;
        Vector3 attackPos = startPos + direction * attackDistance;

        playerAnimator.SetFloat("horizontal", direction.x);
        yield return MoveToPosition(player.transform, attackPos);

        playerAnimator.SetFloat("horizontal", 0f);
        playerAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDuration);

        int baseDamage = Mathf.Max(1, player.strength - (currentEnemy.defense / 2));
        
        // Coup critique (10% de chance)
        bool isCrit = Random.Range(0, 100) < 10;
        int damageToEnemy = isCrit ? Mathf.RoundToInt(baseDamage * 1.5f) : baseDamage;
        
        if (isCrit)
            Debug.Log($"💥 COUP CRITIQUE ! {damageToEnemy} dégâts !");
        
        currentEnemy.currentHP -= damageToEnemy;
        UpdateUI();

        Vector3 returnDir = (startPos - attackPos).normalized;
        playerAnimator.SetFloat("horizontal", returnDir.x);
        yield return MoveToPosition(player.transform, startPos);
        playerAnimator.SetFloat("horizontal", 0f);

        if (currentEnemy.currentHP <= 0)
        {
            player.GainXP(currentEnemy.xpReward);
            EndCombat();
            yield break;
        }


        yield return StartCoroutine(EnemyAttackCoroutine());

        attackButton.interactable = true;
        if (magicButton != null)
            magicButton.interactable = true;

        if (player.currentHealth <= 0)
        {
            if (gameOverManager != null)
                gameOverManager.ShowGameOver();
            else
                EndCombat();
        }
    }

    IEnumerator EnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        // Régénération mana (+10 par tour, max au plafond)
        int manaRegen = 10;
        int oldMana = player.currentMana;
        player.currentMana = Mathf.Min(player.maxMana, player.currentMana + manaRegen);
        
        if (GameManager.instance != null)
            GameManager.instance.currentMana = player.currentMana;
        
        int gainedMana = player.currentMana - oldMana;
        if (gainedMana > 0)
            Debug.Log($"🔵 +{gainedMana} mana régénérée !");
        
        UpdateUI();

        Vector3 startPos = enemyStartPos.position;
        Vector3 direction = (playerStartPos.position - startPos).normalized;
        Vector3 attackPos = startPos + direction * attackDistance;

        yield return MoveToPosition(enemyTransform, attackPos);

        float dodgeChance = player.agility * 2f;
        if (Random.Range(0f, 100f) < dodgeChance)
        {
            Debug.Log("💨 Le joueur esquive l'attaque !");
        }
        else
        {
            int baseDamage = Mathf.Max(1, currentEnemy.attack - (player.defense / 2));
            
            // Coup critique de l'ennemi (chance variable selon le type)
            bool isCrit = Random.Range(0, 100) < currentEnemy.critChance;
            int damageToPlayer = isCrit ? Mathf.RoundToInt(baseDamage * 1.5f) : baseDamage;
            
            if (isCrit)
                Debug.Log($"💀 COUP CRITIQUE ENNEMI ! {damageToPlayer} dégâts !");
            
            player.currentHealth -= damageToPlayer;
            Debug.Log($"💥 Le joueur prend {damageToPlayer} dégâts !");
        }

        UpdateUI();

        yield return new WaitForSeconds(0.3f);

        yield return MoveToPosition(enemyTransform, startPos);
    }

    IEnumerator MoveToPosition(Transform obj, Vector3 target)
    {
        Vector3 startPos = obj.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * attackSpeed;
            obj.position = Vector3.Lerp(startPos, target, t);
            yield return null;
        }
    }

    void EndCombat()
    {
        // Loot aléatoire (40% de chance)
        if (Random.Range(0, 100) < 40)
        {
            GiveLoot();
        }
        
        if (GameManager.instance != null)
        {
            GameManager.instance.currentHealth = player.currentHealth;
            GameManager.instance.currentMana = player.currentMana;
            GameManager.instance.strength = player.strength;
        }

        SceneManager.LoadScene("SampleScene");
    }
    
    void GiveLoot()
    {
        if (Inventory.instance == null) return;
        
        // Loot aléatoire
        int lootType = Random.Range(0, 3);
        
        Item lootedItem = null;
        
        switch (lootType)
        {
            case 0:
                lootedItem = new Item("Potion de soin", ItemType.HealthPotion, 50, "Restaure 50 PV");
                Debug.Log("🎁 LOOT : Potion de soin trouvée !");
                break;
            case 1:
                lootedItem = new Item("Potion de mana", ItemType.ManaPotion, 30, "Restaure 30 mana");
                Debug.Log("🎁 LOOT : Potion de mana trouvée !");
                break;
            case 2:
                lootedItem = new Item("Potion de force", ItemType.StrengthBoost, 5, "+5 force");
                Debug.Log("🎁 LOOT : Potion de force trouvée !");
                break;
        }
        
        if (lootedItem != null)
        {
            Inventory.instance.AddItem(lootedItem);
        }
    }
}
