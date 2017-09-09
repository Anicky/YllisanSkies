using RaverSoft.YllisanSkies.Items;
using RaverSoft.YllisanSkies.Battles;
using System;

namespace RaverSoft.YllisanSkies.Characters
{
    [Serializable]
    public abstract class Character
    {
        public string name;
        public string id { get; protected set; }

        // Attributes
        public int hp { get; private set; }
        public int hpMax { get; private set; }
        public int ap { get; private set; }
        public int apMax { get; private set; }
        public int strength { get; private set; }
        public int strengthMax { get; private set; }
        public int resistance { get; private set; }
        public int resistanceMax { get; private set; }
        public int potential { get; private set; }
        public int potentialMax { get; private set; }
        public int spirit { get; private set; }
        public int spiritMax { get; private set; }
        public int agility { get; private set; }
        public int agilityMax { get; private set; }
        public int cp { get; private set; }
        public int cpMax { get; private set; }

        // Elements
        public int elementFire { get; private set; }
        public int elementFireMax { get; private set; }
        public int elementAir { get; private set; }
        public int elementAirMax { get; private set; }
        public int elementLightning { get; private set; }
        public int elementLightningMax { get; private set; }
        public int elementLight { get; private set; }
        public int elementLightMax { get; private set; }
        public int elementWater { get; private set; }
        public int elementWaterMax { get; private set; }
        public int elementEarth { get; private set; }
        public int elementEarthMax { get; private set; }
        public int elementNature { get; private set; }
        public int elementNatureMax { get; private set; }
        public int elementDarkness { get; private set; }
        public int elementDarknessMax { get; private set; }

        // Battles
        public int currentBattleSpeed = 0;
        public BattleSystem.BattleStates currentBattleState = BattleSystem.BattleStates.Wait;
        public float currentBattlePosition = 0;
        public BattleCommand currentBattleCommand;

        public Character(string name, int hp, int ap, int strength, int resistance, int potential, int spirit, int agility, int cp, int elementFire, int elementAir, int elementLightning, int elementLight, int elementWater, int elementEarth, int elementNature, int elementDarkness)
        {
            this.name = name;
            hpMax = hp;
            apMax = ap;
            strengthMax = strength;
            resistanceMax = resistance;
            potentialMax = potential;
            spiritMax = spirit;
            agilityMax = agility;
            cpMax = cp;
            elementFireMax = elementFire;
            elementAirMax = elementAir;
            elementLightningMax = elementLightning;
            elementLightMax = elementLight;
            elementWaterMax = elementWater;
            elementEarthMax = elementEarth;
            elementNatureMax = elementNature;
            elementDarknessMax = elementDarkness;

            this.hp = hp;
            this.ap = ap;
            this.strength = strength;
            this.resistance = resistance;
            this.potential = potential;
            this.spirit = spirit;
            this.agility = agility;
            this.cp = cp;
            this.elementFire = elementFire;
            this.elementAir = elementAir;
            this.elementLightning = elementLightning;
            this.elementLight = elementLight;
            this.elementWater = elementWater;
            this.elementEarth = elementEarth;
            this.elementNature = elementNature;
            this.elementDarkness = elementDarkness;
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

        protected void equipStatItem(StatItem statItem)
        {
            changeStatsFromStatItem(statItem, 1);
        }

        protected void unequipStatItem(StatItem statItem)
        {
            changeStatsFromStatItem(statItem, -1);
        }

        protected void changeStatsFromStatItem(StatItem statItem, int multiplier)
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
}