using UnityEngine;

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

    // Adjusting parameters
    private const int POINTS_ATTRIBUTES_TO_ADD_FOR_EACH_LV = 10;
    private const int POINTS_ELEMENTS_TO_ADD_FOR_EACH_LV = 10;

    public Hero(string name, int lv, int hp, int hpMax, int ap, int apMax)
    {
        this.name = name;
        this.lv = lv;
        this.hp = hp;
        this.hpMax = hpMax;
        this.ap = ap;
        this.apMax = apMax;
        xpToNextLv = getXpNeededToNextLevel();
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

    public int getHp()
    {
        return hp;
    }

    public int getHpMax()
    {
        return hpMax;
    }

    public int getAp()
    {
        return ap;
    }

    public int getApMax()
    {
        return apMax;
    }

    private int changeStat(int currentValue, int max, int points, int multiplier)
    {
        currentValue += points * multiplier;
        if (currentValue < 0)
        {
            currentValue = 0;
        }
        else if (currentValue > max)
        {
            currentValue = max;
        }
        return currentValue;
    }

    public void addHpMax(int hpMaxToAdd)
    {
        hpMax += hpMaxToAdd;
        if (hp > 0)
        {
            hp += hpMaxToAdd;
        }
    }

    public void addApMax(int apMaxToAdd)
    {
        apMax += apMaxToAdd;
        if (ap > 0)
        {
            ap += apMaxToAdd;
        }
    }

    public void removeHpMax(int hpMaxToRemove)
    {
        hpMax -= hpMaxToRemove;
        if (hp > hpMax)
        {
            hp = hpMax;
        }
    }

    public void removeApMax(int apMaxToRemove)
    {
        apMax -= apMaxToRemove;
        if (ap > apMax)
        {
            ap += apMaxToRemove;
        }
    }

    public void addHp(int hpToAdd)
    {
        hp = changeStat(hp, hpMax, hpToAdd, 1);
    }

    public void removeHp(int hpToRemove)
    {
        hp = changeStat(hp, hpMax, hpToRemove, -1);
    }

    public void addAp(int apToAdd)
    {
        ap = changeStat(ap, apMax, apToAdd, 1);
    }

    public void removeAp(int apToAdd)
    {
        ap = changeStat(ap, apMax, apToAdd, -1);
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

    private void equipStatItem(StatItem statItem)
    {
        changeStatsFromStatItem(statItem, 1);
    }

    private void unequipStatItem(StatItem statItem)
    {
        changeStatsFromStatItem(statItem, -1);
    }

    private void changeStatsFromStatItem(StatItem statItem, int multiplier)
    {
        // Attributes
        hp += statItem.hpMaxToIncrease * multiplier;
        hpMax += statItem.hpMaxToIncrease * multiplier;
        ap += statItem.apMaxToIncrease * multiplier;
        apMax += statItem.apMaxToIncrease * multiplier;
        strength += statItem.strengthMaxToIncrease * multiplier;
        strengthMax += statItem.strengthMaxToIncrease * multiplier;
        resistance += statItem.resistanceMaxToIncrease * multiplier;
        resistanceMax += statItem.resistanceMaxToIncrease * multiplier;
        potential += statItem.potentialMaxToIncrease * multiplier;
        potentialMax += statItem.potentialMaxToIncrease * multiplier;
        spirit += statItem.spiritMaxToIncrease * multiplier;
        spiritMax += statItem.spiritMaxToIncrease * multiplier;
        agility += statItem.agilityMaxToIncrease * multiplier;
        agilityMax += statItem.agilityMaxToIncrease * multiplier;
        cp += statItem.cpMaxToIncrease * multiplier;
        cpMax += statItem.cpMaxToIncrease * multiplier;

        // Elements
        elementFire += statItem.elementFireMaxToIncrease * multiplier;
        elementFireMax += statItem.elementFireMaxToIncrease * multiplier;
        elementAir += statItem.elementAirMaxToIncrease * multiplier;
        elementAirMax += statItem.elementAirMaxToIncrease * multiplier;
        elementLightning += statItem.elementLightningMaxToIncrease * multiplier;
        elementLightningMax += statItem.elementLightningMaxToIncrease * multiplier;
        elementLight += statItem.elementLightMaxToIncrease * multiplier;
        elementLightMax += statItem.elementLightMaxToIncrease * multiplier;
        elementWater += statItem.elementWaterMaxToIncrease * multiplier;
        elementWaterMax += statItem.elementWaterMaxToIncrease * multiplier;
        elementEarth += statItem.elementEarthMaxToIncrease * multiplier;
        elementEarthMax += statItem.elementEarthMaxToIncrease * multiplier;
        elementNature += statItem.elementNatureMaxToIncrease * multiplier;
        elementNatureMax += statItem.elementNatureMaxToIncrease * multiplier;
        elementDarkness += statItem.elementDarknessMaxToIncrease * multiplier;
        elementDarknessMax += statItem.elementDarknessMaxToIncrease * multiplier;
    }

}
