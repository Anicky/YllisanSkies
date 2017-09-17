using UnityEngine;

namespace RaverSoft.YllisanSkies.Events
{
    public class LoadMap : Event
    {
        public enum TransitionsEffects
        {
            None,
            Fade,
            Battle_01
        }

        public string mapToLoad;
        public Vector3 playerStartingPoint;
        public TransitionsEffects transitionEffectIn = TransitionsEffects.Fade;
        public TransitionsEffects transitionEffectOut = TransitionsEffects.Fade;

        protected override void doActionWhenTriggered()
        {
            StartCoroutine(game.changeScene(mapToLoad, playerStartingPoint, transitionEffectIn, transitionEffectOut));
        }

        protected override void doActionWhenExitTrigger()
        {

        }
    }
}