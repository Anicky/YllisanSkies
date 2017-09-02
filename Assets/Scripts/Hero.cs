public class Hero
{
    public string name;

    // Level
    public int lv;
    public int xp;

    // Equipment
    public WeaponItem weapon;
    public Protectiontem protection;
    public AccessoryItem accessory1;
    public AccessoryItem accessory2;

    // Attributes
    public int hp;
    public int hpMax;
    public int ap;
    public int apMax;
    public int strength;
    public int strengthMax;
    public int resistance;
    public int resistanceMax;
    public int potential;
    public int potentialMax;
    public int spirit;
    public int spiritMax;
    public int agility;
    public int agilityMax;
    public int cp;
    public int cpMax;

    // Elements
    public int elementFire;
    public int elementFireMax;
    public int elementAir;
    public int elementAirMax;
    public int elementLightning;
    public int elementLightningMax;
    public int elementLight;
    public int elementLightMax;
    public int elementWater;
    public int elementWaterMax;
    public int elementEarth;
    public int elementEarthMax;
    public int elementNature;
    public int elementNatureMax;
    public int elementDarkness;
    public int elementDarknessMax;

    public Hero(string name)
    {
        this.name = name;
    }

}
