using RaverSoft.YllisanSkies.Abilities;

namespace RaverSoft.YllisanSkies.Battles
{
    public class BattleAbilitiesCommand : BattleCommand
    {
        private Ability ability;

        public BattleAbilitiesCommand(Ability ability)
        {
            this.ability = ability;
        }
    }
}