using RaverSoft.YllisanSkies.Items;
using System.Collections.Generic;

namespace RaverSoft.YllisanSkies.Characters
{
    public class HeroesTeam
    {
        private List<Hero> heroes;
        private int moneyCollected = 0;
        private int currentMoney = 0;
        private List<Item> items;
        private Location currentLocation;

        public const int MAXIMUM_NUMBER_OF_HEROES = 4;

        public HeroesTeam()
        {
            heroes = new List<Hero>();
            items = new List<Item>();
        }

        public List<Hero> getHeroes()
        {
            return heroes;
        }

        public int getNumberOfHeroes()
        {
            return heroes.Count;
        }

        public Hero getHeroByIndex(int i)
        {
            Hero hero = null;
            if (i >= 0 && i < heroes.Count)
            {
                hero = heroes[i];
            }
            return hero;
        }

        public void addHero(Hero hero)
        {
            if (heroes.Count < MAXIMUM_NUMBER_OF_HEROES)
            {
                heroes.Add(hero);
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

        public void initBattle()
        {
            foreach (Hero hero in heroes)
            {
                hero.initBattle();
            }
        }
    }
}