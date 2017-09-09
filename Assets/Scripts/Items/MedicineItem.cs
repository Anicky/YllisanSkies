using System;

namespace RaverSoft.YllisanSkies.Items
{
    [Serializable]
    public class MedicineItem : Item
    {
        public bool usableInMenu = true;
        private int hpToRestore = 0;
        private int hpPercentageToRestore = 0;
        private int apToRestore = 0;
        private int apPercentageToRestore = 0;
    }
}
