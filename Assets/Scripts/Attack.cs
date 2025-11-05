using UnityEngine;

[System.Serializable]
public enum AttackType
{
    Physical,
    Magic
}

[System.Serializable]
public class Attack
{
    public string attackName;
    public AttackType type;
    public int damage;
    public int manaCost;
    public int accuracy = 100;
    public int critChance = 10;
    
    public Attack(string name, AttackType attackType, int dmg, int mana = 0, int acc = 100, int crit = 10)
    {
        attackName = name;
        type = attackType;
        damage = dmg;
        manaCost = mana;
        accuracy = acc;
        critChance = crit;
    }
}
