using RaverSoft.YllisanSkies.Items;
using System.Collections.Generic;

namespace RaverSoft.YllisanSkies.Characters
{
    public class HeroesTeam : CharactersTeam
    {
        private int moneyCollected = 0;
        private int currentMoney = 0;
        private List<Item> items;
        private Location currentLocation;

        public const int MAXIMUM_NUMBER_OF_HEROES = 4;

        public HeroesTeam() : base(MAXIMUM_NUMBER_OF_HEROES)
        {
            items = new List<Item>();
        }

        public int getCurrentMoney()
        {
            return currentMoney;
        }

        public void addMoney(int moneyToAdd)
        {
            moneyCollected += moneyToAdd;
            currentMoney += moneyToAdd;
        }

        public void removeMoney(int moneyToRemove)
        {
            currentMoney -= moneyToRemove;
        }

        public void addItem(Item item)
        {
            items.Add(item);
        }

        public Location getCurrentLocation()
        {
            return currentLocation;
        }

        public void changeLocation(Location location)
        {
            currentLocation = location;
        }

        public void initBattle()
        {
            foreach (Hero hero in characters)
            {
                hero.initBattle();
            }
        }
    }
}