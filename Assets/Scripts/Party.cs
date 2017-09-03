using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Items;
using System.Collections.Generic;

namespace RaverSoft.YllisanSkies
{
    public class Party
    {
        private Hero[] heroes;
        private int moneyCollected = 0;
        private int currentMoney = 0;
        private List<Item> items;
        private Location currentLocation;

        public Party()
        {
            heroes = new Hero[] { null, null, null, null };
        }

        public Hero[] getHeroes()
        {
            return heroes;
        }

        public int getNumberOfHeroes()
        {
            int numberOfHeroes = 0;
            for (int i = 0; i < heroes.Length; i++)
            {
                if (heroes[i] != null)
                {
                    numberOfHeroes++;
                }
            }
            return numberOfHeroes;
        }

        public Hero getHeroByIndex(int i)
        {
            Hero hero = null;
            if (i >= 0 && i < heroes.Length)
            {
                hero = heroes[i];
            }
            return hero;
        }

        public void addHero(Hero hero)
        {
            bool heroAdded = false;
            for (int i = 0; i < heroes.Length; i++)
            {
                if (heroes[i] == null)
                {
                    heroes[i] = hero;
                    heroAdded = true;
                    break;
                }
            }
            if (!heroAdded)
            {
                // @TODO : Throw exception
            }
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
    }
}