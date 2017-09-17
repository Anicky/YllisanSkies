using UnityEngine;
using RaverSoft.YllisanSkies.Events.Commands;
using RaverSoft.YllisanSkies.Sound;

namespace RaverSoft.YllisanSkies.Events
{
    public class SavePoint : Event
    {
        protected override void doActionWhenTriggered()
        {
            game.playSound(Sounds.Submit);
            EventCommandFlash.createComponent(gameObject, game, Color.white, 0.3f, true, 0.3f).init();
            game.isSaveAllowed = true;
        }

        protected override void doActionWhenExitTrigger()
        {
            game.isSaveAllowed = false;
        }
    }
}