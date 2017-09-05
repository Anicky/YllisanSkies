using System.Collections.Generic;
using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Utils;
using RaverSoft.YllisanSkies.Items;
using RaverSoft.YllisanSkies.Abilities;
using System;

namespace RaverSoft.YllisanSkies
{
    public class Database
    {
        private Dictionary<LanguageList, Language> languages;
        private Dictionary<HeroList, Hero> heroes;
        private Dictionary<LocationList, Location> locations;
        private Dictionary<BehaviourList, Behaviour> behaviours;
        private Dictionary<EnemyList, Enemy> enemies;
        private Dictionary<ItemList, Item> items;
        private Dictionary<AbilityList, Ability> abilities;

        public Database()
        {
            languages = new Dictionary<LanguageList, Language>();
            heroes = new Dictionary<HeroList, Hero>();
            locations = new Dictionary<LocationList, Location>();
            behaviours = new Dictionary<BehaviourList, Behaviour>();
            enemies = new Dictionary<EnemyList, Enemy>();
            items = new Dictionary<ItemList, Item>();
            abilities = new Dictionary<AbilityList, Ability>();
        }

        public void load()
        {
            loadItems();
            loadAbilities();
            loadLanguages();
            loadHeroes();
            loadLocations();
            loadBehaviours();
            loadEnemies();
        }

        private List<Dictionary<string, string>> getCSVinfo(string type)
        {
            return CSVReader.getItemsFromFile("Database/" + type);
        }

        private void loadItems()
        {
            // @TODO : create MedicineItems file
            // @TODO : create PermanentItems file
            // @TODO : create WeaponItems file
            // @TODO : create ProtectionItems file
            // @TODO : create AccessoryItems file
            // @TODO : create MaterialItems file
            // @TODO : create BattleItems file
            // @TODO : create MiscellaneousItems file
        }

        private void loadAbilities()
        {
            // @TODO : create Abilities file
        }

        private void loadHeroes()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Heroes"))
            {
                heroes.Add((HeroList)Enum.Parse(typeof(HeroList), info["id"]), new Hero(
                    info["id"],
                    info["name"],
                    int.Parse(info["lv"]),
                    int.Parse(info["xpSlopeToIncreaseLevel"]),
                    int.Parse(info["xpVerticalInterceptToIncreaseLevel"]),
                    int.Parse(info["hp"]),
                    int.Parse(info["ap"]),
                    int.Parse(info["strength"]),
                    int.Parse(info["resistance"]),
                    int.Parse(info["potential"]),
                    int.Parse(info["spirit"]),
                    int.Parse(info["agility"]),
                    int.Parse(info["cp"]),
                    int.Parse(info["elementFire"]),
                    int.Parse(info["elementAir"]),
                    int.Parse(info["elementLightning"]),
                    int.Parse(info["elementLight"]),
                    int.Parse(info["elementWater"]),
                    int.Parse(info["elementEarth"]),
                    int.Parse(info["elementNature"]),
                    int.Parse(info["elementDarkness"])
                ));
            }
        }

        private void loadLanguages()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Languages"))
            {
                languages.Add((LanguageList)Enum.Parse(typeof(LanguageList), info["id"]), new Language(
                    info["id"],
                    info["name"]
                ));
            }
        }

        private void loadLocations()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Locations"))
            {
                locations.Add((LocationList)Enum.Parse(typeof(LocationList), info["id"]), new Location(
                    info["id"],
                    info["name"]
                ));
            }
        }

        private void loadBehaviours()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Behaviours"))
            {
                behaviours.Add((BehaviourList)Enum.Parse(typeof(BehaviourList), info["id"]), new Behaviour(
                    info["id"],
                    info["name"]
                ));
            }
        }

        private void loadEnemies()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Enemies"))
            {
                Behaviour enemyBehaviour = null;
                if (info["behaviour"] != "")
                {
                    enemyBehaviour = getBehaviourById((BehaviourList)Enum.Parse(typeof(BehaviourList), info["behaviour"]));
                }
                Item commonItem = null;
                if (info["commonItem"] != "")
                {
                    commonItem = getItemById((ItemList)Enum.Parse(typeof(ItemList), info["commonItem"]));
                }
                Item uncommonItem = null;
                if (info["uncommonItem"] != "")
                {
                    uncommonItem = getItemById((ItemList)Enum.Parse(typeof(ItemList), info["commonItem"]));
                }
                Item rareItem = null;
                if (info["rareItem"] != "")
                {
                    rareItem = getItemById((ItemList)Enum.Parse(typeof(ItemList), info["commonItem"]));
                }

                enemies.Add((EnemyList)Enum.Parse(typeof(EnemyList), info["id"]), new Enemy(
                    info["id"],
                    info["name"],
                    int.Parse(info["hp"]),
                    int.Parse(info["ap"]),
                    int.Parse(info["money"]),
                    int.Parse(info["xp"]),
                    enemyBehaviour,
                    int.Parse(info["strength"]),
                    int.Parse(info["resistance"]),
                    int.Parse(info["potential"]),
                    int.Parse(info["spirit"]),
                    int.Parse(info["agility"]),
                    int.Parse(info["cp"]),
                    int.Parse(info["elementFire"]),
                    int.Parse(info["elementAir"]),
                    int.Parse(info["elementLightning"]),
                    int.Parse(info["elementLight"]),
                    int.Parse(info["elementWater"]),
                    int.Parse(info["elementEarth"]),
                    int.Parse(info["elementNature"]),
                    int.Parse(info["elementDarkness"]),
                    commonItem,
                    uncommonItem,
                    rareItem
                ));
            }
        }

        public Hero getHeroById(HeroList id)
        {
            return heroes[id];
        }

        public Language getLanguageById(LanguageList id)
        {
            return languages[id];
        }

        public Location getLocationById(LocationList id)
        {
            return locations[id];
        }

        public Behaviour getBehaviourById(BehaviourList id)
        {
            return behaviours[id];
        }

        public Enemy getEnemyById(EnemyList id)
        {
            return enemies[id].clone();
        }

        public Item getItemById(ItemList id)
        {
            return items[id];
        }

        public Ability getAbilityById(AbilityList id)
        {
            return abilities[id];
        }

    }
}