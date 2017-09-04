using RaverSoft.YllisanSkies.Items;

namespace RaverSoft.YllisanSkies.Characters
{
    public abstract class Character
    {
        public string name;

        // Attributes
        protected int hp;
        protected int hpMax;
        protected int ap;
        protected int apMax;
        protected int strength;
        protected int strengthMax;
        protected int resistance;
        protected int resistanceMax;
        protected int potential;
        protected int potentialMax;
        protected int spirit;
        protected int spiritMax;
        protected int agility;
        protected int agilityMax;
        protected int cp;
        protected int cpMax;

        // Elements
        protected int elementFire;
        protected int elementFireMax;
        protected int elementAir;
        protected int elementAirMax;
        protected int elementLightning;
        protected int elementLightningMax;
        protected int elementLight;
        protected int elementLightMax;
        protected int elementWater;
        protected int elementWaterMax;
        protected int elementEarth;
        protected int elementEarthMax;
        protected int elementNature;
        protected int elementNatureMax;
        protected int elementDarkness;
        protected int elementDarknessMax;

        // Battles
        public int battleSpeed = 0;

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

        public int getAgility()
        {
            return agility;
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