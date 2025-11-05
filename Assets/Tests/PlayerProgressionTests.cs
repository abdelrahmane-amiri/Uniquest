using NUnit.Framework;
using UnityEngine;

namespace RPG.Tests
{
    public class PlayerProgressionTests
    {
        [Test]
        public void MultipleXPGains_AccumulateCorrectly()
        {
            // Arrange
            int initialXP = 0;
            int[] xpGains = { 25, 30, 45 };

            // Act
            int totalXP = initialXP;
            foreach (int gain in xpGains)
            {
                totalXP += gain;
            }

            // Assert
            Assert.AreEqual(100, totalXP);
        }

        [Test]
        public void MultipleLevelUps_InSingleBattle()
        {
            // Arrange
            int currentXP = 90;
            int xpGain = 150; // Assez pour 2 level ups
            int xpToNextLevel = 100;
            int level = 1;

            // Act
            currentXP += xpGain; // 240 XP total
            int levelUps = 0;

            while (currentXP >= xpToNextLevel)
            {
                currentXP -= xpToNextLevel;
                level++;
                levelUps++;
                xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.3f);
            }

            // Assert
            Assert.AreEqual(2, levelUps);
            Assert.AreEqual(3, level);
        }

        [Test]
        public void StatIncrease_OnLevelUp()
        {
            // Arrange
            int initialStrength = 9;
            int initialDefense = 8;
            int initialMaxHealth = 100;

            // Act (Level up)
            int newStrength = initialStrength + 2;
            int newDefense = initialDefense + 1;
            int newMaxHealth = initialMaxHealth + 10;

            // Assert
            Assert.AreEqual(11, newStrength);
            Assert.AreEqual(9, newDefense);
            Assert.AreEqual(110, newMaxHealth);
        }

        [Test]
        public void HealthRestoration_OnLevelUp()
        {
            // Arrange
            int maxHealth = 100;
            int currentHealth = 30; // Joueur blessé
            
            // Act (Level up)
            maxHealth += 10;
            currentHealth = maxHealth; // Full heal

            // Assert
            Assert.AreEqual(110, currentHealth);
            Assert.AreEqual(maxHealth, currentHealth);
        }
    }
}
