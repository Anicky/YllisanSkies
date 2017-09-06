using RaverSoft.YllisanSkies.Characters;

namespace RaverSoft.YllisanSkies.Events
{
    public class TestBattle : LoadMap
    {
        protected override void doActionWhenTriggered()
        {
            game.inBattle = true;
            game.enemiesTeam.addCharacter(game.getDatabase().getEnemyById(EnemyList.RoyalEagle));
            game.enemiesTeam.addCharacter(game.getDatabase().getEnemyById(EnemyList.LoneWolf));
            game.enemiesTeam.addCharacter(game.getDatabase().getEnemyById(EnemyList.RoyalEagle));
            base.doActionWhenTriggered();
        }
    }
}