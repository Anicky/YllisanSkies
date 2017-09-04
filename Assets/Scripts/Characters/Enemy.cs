using RaverSoft.YllisanSkies.Items;

namespace RaverSoft.YllisanSkies.Characters
{
    public enum EnemyList
    {
        RoyalEagle,
        LoneWolf
    }

    public class Enemy : Character
    {
        private string id;
        private Behaviour behaviour;
        private int money;
        private int xp;
        private Item commonItem;
        private Item uncommonItem;
        private Item rareItem;

        public Enemy(string id, string name, int hp, int ap, int money, int xp, Behaviour behaviour, int strength, int resistance, int potential, int spirit, int agility, int cp, int elementFire, int elementAir, int elementLightning, int elementLight, int elementWater, int elementEarth, int elementNature, int elementDarkness, Item commonItem, Item uncommonItem, Item rareItem) :
            base(name, hp, ap, strength, resistance, potential, spirit, agility, cp, elementFire, elementAir, elementLightning, elementLight, elementWater, elementEarth, elementNature, elementDarkness)
        {
            this.id = id;
            this.behaviour = behaviour;
            this.money = money;
            this.xp = xp;
            this.commonItem = commonItem;
            this.uncommonItem = uncommonItem;
            this.rareItem = rareItem;
        }

        public string getId()
        {
            return id;
        }

    }
}