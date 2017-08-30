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
        foreach (Transform child in transform)
        {
            if (child.name == "Block")
            {
                player.moveToPosition(child.transform.position);
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
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
