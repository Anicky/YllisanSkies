using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapOpeningDoor : LoadMap
{

    public AudioClip doorSound;

    protected override void doActionWhenTriggered()
    {
        Animator anim = GetComponent<Animator>();
        if (anim)
        {
            anim.SetTrigger("Open");
        }
        // @TODO : Move player 
        // @TODO : Load map when animation is finished
    }
}
