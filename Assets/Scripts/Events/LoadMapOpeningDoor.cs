using UnityEngine;
using System.Collections;

public class LoadMapOpeningDoor : LoadMap
{
    public AudioClip doorSound;

    protected override void doActionWhenTriggered()
    {
        game.menuAllowed = false;
        if (doorSound)
        {
            GameObject.Find("Game").GetComponent<AudioSource>().PlayOneShot(doorSound);
        }
        Animator anim = GetComponent<Animator>();
        if (anim)
        {
            anim.SetTrigger("Open");
        }
        transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        GameObject.Find("Map").GetComponent<Grid>().checkNodesConnections();
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
