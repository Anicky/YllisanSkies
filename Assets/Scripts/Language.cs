namespace RaverSoft.YllisanSkies
{
    public enum LanguageList
    {
        English,
        French
    }

    public class Language
    {
        public string id;
        public string name;

        public Language(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }

}
