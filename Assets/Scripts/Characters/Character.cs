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
    }
}