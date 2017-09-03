using System.Collections.Generic;
using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Utils;

namespace RaverSoft.YllisanSkies
{
    public class Database
    {
        private Dictionary<string, Language> languages;
        private Dictionary<string, Hero> heroes;

        public Database()
        {
            languages = new Dictionary<string, Language>();
            heroes = new Dictionary<string, Hero>();
        }

        public void load()
        {
            loadLanguages();
            loadHeroes();
        }

        private List<Dictionary<string, string>> getCSVinfo(string type)
        {
            return CSVReader.getItemsFromFile("Database/" + type);
        }

        private void loadHeroes()
        {
            foreach (Dictionary<string, string> info in getCSVinfo("Heroes"))
            {
                heroes.Add(info["id"], new Hero(
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
                languages.Add(info["id"], new Language(
                    info["id"],
                    info["name"]
                ));
            }
        }

        public Hero getHeroById(string id)
        {
            return heroes[id];
        }

        public Language getLanguageById(string id)
        {
            return languages[id];
        }

    }
}