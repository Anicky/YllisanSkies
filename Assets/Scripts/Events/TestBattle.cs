namespace RaverSoft.YllisanSkies.Events
{
    public class TestBattle : LoadMap
    {
        protected override void doActionWhenTriggered()
        {
            game.currentBattle = new Battle(new Enemy[] { new Enemy(), new Enemy(), new Enemy(), new Enemy() });
            base.doActionWhenTriggered();
        }
    }
}