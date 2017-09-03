using System.Collections.Generic;
using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Utils;
using System;

namespace RaverSoft.YllisanSkies
{
    public class Database
    {
        private Dictionary<Languages, Language> languages;
        private Dictionary<Heroes, Hero> heroes;
        private Dictionary<Locations, Location> locations;

        public Database()
        {
            languages = new Dictionary<Languages, Language>();
            heroes = new Dictionary<Heroes, Hero>();
            locations = new Dictionary<Locations, Location>();
        }

        public void load()
        {
            loadLanguages();
            loadHeroes();
            loadLocations();
        }

        private List<Dictionary<string, string>> getCSVinfo(string type)
        {
            return CSVReader.getItemsFromFile("Database/" + type);
        }

        private void loadHeroes()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Heroes"))
            {
                heroes.Add((Heroes)Enum.Parse(typeof(Heroes), info["id"]), new Hero(
                    info["name"],
                    int.Parse(info["lv"]),
                    int.Parse(info["xpSlopeToIncreaseLevel"]),
                    int.Parse(info["xpVerticalInterceptToIncreaseLevel"]),
                    int.Parse(info["hp"]),
                    int.Parse(info["ap"])
                ));
            }
        }

        private void loadLanguages()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Languages"))
            {
                languages.Add((Languages)Enum.Parse(typeof(Languages), info["id"]), new Language(
                    info["id"],
                    info["name"]
                ));
            }
        }

        private void loadLocations()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Locations"))
            {
                locations.Add((Locations)Enum.Parse(typeof(Locations), info["id"]), new Location(
                    info["name"]
                ));
            }
        }

        public Hero getHeroById(Heroes id)
        {
            return heroes[id];
        }

        public Language getLanguageById(Languages id)
        {
            return languages[id];
        }

        public Location getLocationById(Locations id)
        {
            return locations[id];
        }

    }
}