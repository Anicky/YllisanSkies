﻿namespace RaverSoft.YllisanSkies
{
    public enum LocationList
    {
        ForestOfHopes,
        Osarian
    }

    public class Location
    {
        private string name;

        public Location(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }
    }
}
