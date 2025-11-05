using UnityEngine;

[System.Serializable]
public enum EnemyType
{
    Goblin,
    Orc,
    Mage
}

[System.Serializable]
public class Enemy
{
    public string enemyName;
    public EnemyType type;
    public int maxHP;
    public int currentHP;
    public int attack;
    public int defense;
    public int critChance;
    public int xpReward;
    
    public Enemy(EnemyType enemyType)
    {
        type = enemyType;
        
        switch (enemyType)
        {
            case EnemyType.Goblin:
                enemyName = "Goblin";
                maxHP = 30;
                currentHP = 30;
                attack = 8;
                defense = 2;
                critChance = 15;
                xpReward = 20;
                break;
                
            case EnemyType.Orc:
                enemyName = "Orc";
                maxHP = 80;
                currentHP = 80;
                attack = 15;
                defense = 8;
                critChance = 5;
                xpReward = 40;
                break;
                
            case EnemyType.Mage:
                enemyName = "Mage Noir";
                maxHP = 40;
                currentHP = 40;
                attack = 12;
                defense = 3;
                critChance = 10;
                xpReward = 35;
                break;
        }
    }
    
    public static Enemy GetRandomEnemy()
    {
        int rand = Random.Range(0, 3);
        return new Enemy((EnemyType)rand);
    }
}
