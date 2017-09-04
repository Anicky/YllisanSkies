using RaverSoft.YllisanSkies.Characters;

namespace RaverSoft.YllisanSkies.Events
{
    public class TestBattle : LoadMap
    {
        protected override void doActionWhenTriggered()
        {
            game.inBattle = true;
            game.enemiesTeam.addEnemy(game.getDatabase().getEnemyById(EnemyList.RoyalEagle));
            game.enemiesTeam.addEnemy(game.getDatabase().getEnemyById(EnemyList.LoneWolf));
            game.enemiesTeam.addEnemy(game.getDatabase().getEnemyById(EnemyList.RoyalEagle));
            base.doActionWhenTriggered();
        }
    }
}