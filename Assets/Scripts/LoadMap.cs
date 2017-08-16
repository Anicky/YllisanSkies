using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadMap : Event
{

    public enum TransitionsEffects { None, Fade };

    public string mapToLoad;
    public Vector3 playerStartingPoint;
    private bool inTransition = false;
    public TransitionsEffects transitionEffectIn = TransitionsEffects.Fade;
    public TransitionsEffects transitionEffectOut = TransitionsEffects.Fade;
    private Canvas canvas;

    private new void Start()
    {
        base.Start();
        canvas = GameObject.Find("Game/Canvas").GetComponent<Canvas>();
    }

    protected override void doActionWhenTriggered()
    {
        StartCoroutine(changeScene());
    }

    private IEnumerator changeScene()
    {
        StartCoroutine(transition(transitionEffectIn, "In"));
        do
        {
            yield return null;
        } while (inTransition);
        SceneManager.LoadScene(mapToLoad);
        player.transform.position = playerStartingPoint;
        StartCoroutine(transition(transitionEffectOut, "Out"));
    }

    private IEnumerator transition(TransitionsEffects transitionEffect, string transitionType)
    {
        if (transitionType == "In")
        {
            canvas.enabled = true;
        }
        inTransition = true;
        if (transitionEffect != TransitionsEffects.None)
        {
            GameObject game = GameObject.Find("Game");
            Animation anim = game.GetComponent<Animation>();
            anim.Play("Overlay_" + transitionEffect + transitionType);
            do
            {
                yield return null;
            } while (anim.isPlaying);
        }
        inTransition = false;
        if (transitionType == "Out")
        {
            canvas.enabled = false;
        }
    }

}
