namespace RaverSoft.YllisanSkies
{
    public enum LocationList
    {
        ForestOfHopes,
        Osarian
    }

    public class Location
    {
        private string id;
        private string name;

        public Location(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public string getId()
        {
            return id;
        }

        public string getName()
        {
            return name;
        }
    }
}
