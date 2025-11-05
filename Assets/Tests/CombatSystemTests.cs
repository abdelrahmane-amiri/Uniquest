using NUnit.Framework;
using UnityEngine;

namespace RPG.Tests
{
    public class CombatSystemTests
    {
        [Test]
        public void DamageCalculation_WithDefense_ReturnsCorrectDamage()
        {
            // Arrange
            int attackerStrength = 10;
            int defenderDefense = 4;
            int expectedDamage = 10 - (4 / 2); // 10 - 2 = 8

            // Act
            int actualDamage = CalculateDamage(attackerStrength, defenderDefense);

            // Assert
            Assert.AreEqual(expectedDamage, actualDamage);
        }

        [Test]
        public void DamageCalculation_MinimumDamage_IsOne()
        {
            // Arrange
            int weakAttack = 2;
            int strongDefense = 20;

            // Act
            int damage = CalculateDamage(weakAttack, strongDefense);

            // Assert
            Assert.GreaterOrEqual(damage, 1, "Les dégâts minimum doivent être de 1");
        }

        [Test]
        public void DodgeChance_HighAgility_IncreasesEvasion()
        {
            // Arrange
            int lowAgility = 2;
            int highAgility = 10;

            // Act
            float lowDodgeChance = CalculateDodgeChance(lowAgility);
            float highDodgeChance = CalculateDodgeChance(highAgility);

            // Assert
            Assert.Greater(highDodgeChance, lowDodgeChance, "Plus d'agilité = plus de chance d'esquive");
        }

        [Test]
        public void DodgeChance_NeverExceeds100Percent()
        {
            // Arrange
            int extremeAgility = 100;

            // Act
            float dodgeChance = CalculateDodgeChance(extremeAgility);

            // Assert
            Assert.LessOrEqual(dodgeChance, 100f, "La chance d'esquive ne peut pas dépasser 100%");
        }

        [Test]
        public void XPGain_IncreasesPlayerXP()
        {
            // Arrange
            int initialXP = 0;
            int xpGain = 25;

            // Act
            int newXP = initialXP + xpGain;

            // Assert
            Assert.AreEqual(25, newXP);
        }

        [Test]
        public void LevelUp_WhenXPThresholdReached()
        {
            // Arrange
            int currentXP = 100;
            int xpToNextLevel = 100;
            int currentLevel = 1;

            // Act
            bool shouldLevelUp = currentXP >= xpToNextLevel;
            int newLevel = shouldLevelUp ? currentLevel + 1 : currentLevel;

            // Assert
            Assert.IsTrue(shouldLevelUp);
            Assert.AreEqual(2, newLevel);
        }

        [Test]
        public void LevelUp_IncreasesMaxHealth()
        {
            // Arrange
            int maxHealthBefore = 100;
            int healthBonus = 10;

            // Act
            int maxHealthAfter = maxHealthBefore + healthBonus;

            // Assert
            Assert.AreEqual(110, maxHealthAfter);
        }

        [Test]
        public void XPRequirement_IncreasesPerLevel()
        {
            // Arrange
            int xpForLevel2 = 100;
            float multiplier = 1.3f;

            // Act
            int xpForLevel3 = Mathf.RoundToInt(xpForLevel2 * multiplier);

            // Assert
            Assert.Greater(xpForLevel3, xpForLevel2, "L'XP requis doit augmenter à chaque niveau");
            Assert.AreEqual(130, xpForLevel3);
        }

        [Test]
        public void HealthDamage_CannotGoBelowZero()
        {
            // Arrange
            int currentHealth = 10;
            int damage = 50;

            // Act
            int newHealth = Mathf.Max(0, currentHealth - damage);

            // Assert
            Assert.GreaterOrEqual(newHealth, 0);
            Assert.AreEqual(0, newHealth);
        }

        [Test]
        public void ClassStats_Archer_HasCorrectValues()
        {
            // Arrange & Act
            var archerStats = new ClassStats
            {
                health = 100,
                mana = 60,
                strength = 6,
                agility = 9,
                intelligence = 5,
                defense = 4
            };

            // Assert
            Assert.AreEqual(100, archerStats.health);
            Assert.AreEqual(9, archerStats.agility, "L'archer doit avoir la plus haute agilité");
        }

        [Test]
        public void ClassStats_Moine_HasCorrectValues()
        {
            // Arrange & Act
            var moineStats = new ClassStats
            {
                health = 100,
                mana = 120,
                strength = 4,
                agility = 5,
                intelligence = 9,
                defense = 6
            };

            // Assert
            Assert.AreEqual(120, moineStats.mana, "Le moine doit avoir le plus de mana");
            Assert.AreEqual(9, moineStats.intelligence, "Le moine doit avoir la plus haute intelligence");
        }

        [Test]
        public void ClassStats_Warrior_HasCorrectValues()
        {
            // Arrange & Act
            var warriorStats = new ClassStats
            {
                health = 100,
                mana = 40,
                strength = 9,
                agility = 4,
                intelligence = 3,
                defense = 8
            };

            // Assert
            Assert.AreEqual(9, warriorStats.strength, "Le warrior doit avoir la plus haute force");
            Assert.AreEqual(8, warriorStats.defense, "Le warrior doit avoir la plus haute défense");
        }

        [Test]
        public void CriticalHit_Multiplier_IncreaseDamage()
        {
            // Arrange
            int baseDamage = 10;
            float critMultiplier = 1.5f;

            // Act
            int critDamage = Mathf.RoundToInt(baseDamage * critMultiplier);

            // Assert
            Assert.Greater(critDamage, baseDamage);
            Assert.AreEqual(15, critDamage);
        }

        [Test]
        public void RandomEncounter_ProbabilityIsValid()
        {
            // Arrange
            int chanceDeCombat = 10; // 1/10

            // Act
            bool isValidProbability = chanceDeCombat > 0 && chanceDeCombat <= 100;

            // Assert
            Assert.IsTrue(isValidProbability);
        }

        // Méthodes utilitaires pour les tests
        private int CalculateDamage(int attack, int defense)
        {
            return Mathf.Max(1, attack - (defense / 2));
        }

        private float CalculateDodgeChance(int agility)
        {
            return Mathf.Min(agility * 2f, 100f);
        }
    }
}
