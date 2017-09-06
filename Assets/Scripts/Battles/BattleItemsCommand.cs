using RaverSoft.YllisanSkies.Items;

namespace RaverSoft.YllisanSkies.Battles
{
    public class BattleItemsCommand : BattleCommand
    {
        private Item item;

        public BattleItemsCommand(Item item)
        {
            this.item = item;
        }
    }
}