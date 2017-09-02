using UnityEngine;
using System.Collections;

namespace RaverSoft.YllisanSkies.Events
{
    public class LoadMapOpeningDoor : LoadMap
    {
        public AudioClip doorSound;

        protected override void doActionWhenTriggered()
        {
            player.disableMovement();
            game.menuAllowed = false;
            if (doorSound)
            {
                game.GetComponent<AudioSource>().PlayOneShot(doorSound);
            }
            Animator anim = GetComponent<Animator>();
            if (anim)
            {
                anim.SetTrigger("Open");
            }
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            grid.recheckNodeConnection(grid.worldToGrid(transform.position, false));
            player.moveToPosition(transform.position);
        }

        protected void DoorOpenFinished()
        {
            StartCoroutine(waitForPlayerToMove());
        }

        private IEnumerator waitForPlayerToMove()
        {
            do
            {
                yield return null;
            } while (player.isMovingToPosition);
            base.doActionWhenTriggered();
        }
    }
}