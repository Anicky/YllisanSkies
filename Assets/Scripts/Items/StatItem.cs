using System;

namespace RaverSoft.YllisanSkies.Items
{
    [Serializable]
    public class StatItem : Item
    {
        // Attributes
        public int hpMaxToIncrease = 0;
        public int apMaxToIncrease = 0;
        public int strengthMaxToIncrease = 0;
        public int resistanceMaxToIncrease = 0;
        public int potentialMaxToIncrease = 0;
        public int spiritMaxToIncrease = 0;
        public int agilityMaxToIncrease = 0;
        public int cpMaxToIncrease = 0;

        // Elements
        public int elementFireMaxToIncrease = 0;
        public int elementAirMaxToIncrease = 0;
        public int elementLightningMaxToIncrease = 0;
        public int elementLightMaxToIncrease = 0;
        public int elementWaterMaxToIncrease = 0;
        public int elementEarthMaxToIncrease = 0;
        public int elementNatureMaxToIncrease = 0;
        public int elementDarknessMaxToIncrease = 0;
    }
}