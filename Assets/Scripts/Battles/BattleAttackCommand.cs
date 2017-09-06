namespace RaverSoft.YllisanSkies.Battles
{
    public class BattleAttackCommand : BattleCommand
    {
        private int attackPoints;

        public BattleAttackCommand(int attackPoints)
        {
            this.attackPoints = attackPoints;
        }
    }
}