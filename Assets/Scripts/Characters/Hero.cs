using UnityEngine;
using RaverSoft.YllisanSkies.Items;
using System;

namespace RaverSoft.YllisanSkies.Characters
{

    public enum HeroList
    {
        Cyril,
        Max,
        Yuna,
        Leonard,
        Shiro,
        Ashley,
        Natsuky
    }

    [Serializable]
    public class Hero : Character
    {
        // Level
        private int lv = 1;
        private int xpTotal = 0;
        private int xpToNextLv;
        private int xpSlopeToIncreaseLevel = 1;
        private int xpVerticalInterceptToIncreaseLevel = 1;
        public int pointsAttributesToAdd = 0;
        public int pointsElementsToAdd = 0;

        // Equipment
        private WeaponItem weapon;
        private Protectiontem protection;
        private AccessoryItem accessory1;
        private AccessoryItem accessory2;

        // Battles
        private int attackPoints = 0;

        // Adjusting parameters
        private const int POINTS_ATTRIBUTES_TO_ADD_FOR_EACH_LV = 10;
        private const int POINTS_ELEMENTS_TO_ADD_FOR_EACH_LV = 10;
        public const int BATTLE_MAX_ATTACK_POINTS = 8;
        public const int BATTLE_ATTACK_POINTS_TO_ADD_WHEN_COMMAND = 2;

        public Hero(string id, string name, int lv, int xpSlopeToIncreaseLevel, int xpVerticalInterceptToIncreaseLevel, int hp, int ap, int strength, int resistance, int potential, int spirit, int agility, int cp, int elementFire, int elementAir, int elementLightning, int elementLight, int elementWater, int elementEarth, int elementNature, int elementDarkness) :
            base(name, hp, ap, strength, resistance, potential, spirit, agility, cp, elementFire, elementAir, elementLightning, elementLight, elementWater, elementEarth, elementNature, elementDarkness)
        {
            this.id = id;
            this.lv = lv;
            this.xpSlopeToIncreaseLevel = xpSlopeToIncreaseLevel;
            this.xpVerticalInterceptToIncreaseLevel = xpVerticalInterceptToIncreaseLevel;
            xpToNextLv = getXpNeededToNextLevel();
        }

        public void initBattle()
        {
            attackPoints = 0;
        }

        public int getAttackPoints()
        {
            return attackPoints;
        }

        public void initBattleCommand()
        {
            addAttackPoints(BATTLE_ATTACK_POINTS_TO_ADD_WHEN_COMMAND);
        }

        public void addAttackPoints(int attackPoints)
        {
            this.attackPoints += attackPoints;
            if (this.attackPoints > BATTLE_MAX_ATTACK_POINTS)
            {
                this.attackPoints = BATTLE_MAX_ATTACK_POINTS;
            }
        }

        public void removeAttackPoints(int attackPoints)
        {
            this.attackPoints -= attackPoints;
            if (this.attackPoints < 0)
            {
                this.attackPoints = 0;
            }
        }

        public int getLv()
        {
            return lv;
        }

        public int getXpTotal()
        {
            return xpTotal;
        }

        public int getXpToNextLv()
        {
            return xpToNextLv;
        }

        public void addXp(int xpToAdd)
        {
            while (xpToAdd > 0)
            {
                int xpForThisLv = Mathf.Min(xpToAdd, xpToNextLv);
                if (xpForThisLv > 0)
                {
                    xpToAdd -= xpForThisLv;
                    xpTotal += xpForThisLv;
                    xpToNextLv -= xpForThisLv;
                    if (xpToNextLv == 0)
                    {
                        nextLv();
                    }
                }
            }
        }

        private void nextLv()
        {
            lv++;
            xpToNextLv = getXpNeededToNextLevel();
            pointsAttributesToAdd += POINTS_ATTRIBUTES_TO_ADD_FOR_EACH_LV;
            pointsElementsToAdd += POINTS_ELEMENTS_TO_ADD_FOR_EACH_LV;
        }

        private int getXpNeededToNextLevel()
        {
            return (lv * xpSlopeToIncreaseLevel) + xpVerticalInterceptToIncreaseLevel;
        }

        public WeaponItem getWeapon()
        {
            return weapon;
        }

        public void equipWeapon(WeaponItem weapon)
        {
            this.weapon = weapon;
            equipStatItem(weapon);
        }

        public void unequipWeapon()
        {
            unequipStatItem(weapon);
            weapon = null;
        }

        public Protectiontem getProtection()
        {
            return protection;
        }

        public void equipProtection(Protectiontem protection)
        {
            this.protection = protection;
            equipStatItem(protection);
        }

        public void unequipProtection()
        {
            unequipStatItem(protection);
            protection = null;
        }

        public AccessoryItem getAccessory1()
        {
            return accessory1;
        }

        public void equipAccessory1(AccessoryItem accessory)
        {
            accessory1 = accessory;
            equipStatItem(accessory);
        }

        public void unequipAccessory1()
        {
            unequipStatItem(accessory1);
            accessory1 = null;
        }

        public AccessoryItem getAccessory2()
        {
            return accessory2;
        }

        public void equipAccessory2(AccessoryItem accessory)
        {
            accessory2 = accessory;
            equipStatItem(accessory);
        }

        public void unequipAccessory2()
        {
            unequipStatItem(accessory2);
            accessory2 = null;
        }

    }
}