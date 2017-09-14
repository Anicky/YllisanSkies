using System.Globalization;

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
        public string locale;
        private CultureInfo cultureInfo;

        public Language(string id, string name, string locale)
        {
            this.id = id;
            this.name = name;
            this.locale = locale;
        }

        public CultureInfo getCultureInfo()
        {
            return new CultureInfo(locale);
        }
    }

}
