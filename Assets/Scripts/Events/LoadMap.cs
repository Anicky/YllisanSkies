using UnityEngine;

public class LoadMap : Event
{

    public enum TransitionsEffects { None, Fade };

    public string mapToLoad;
    public Vector3 playerStartingPoint;
    private bool inTransition = false;
    public TransitionsEffects transitionEffectIn = TransitionsEffects.Fade;
    public TransitionsEffects transitionEffectOut = TransitionsEffects.Fade;

    private new void Start()
    {
        base.Start();
    }

    protected override void doActionWhenTriggered()
    {
        StartCoroutine(game.changeScene(mapToLoad, playerStartingPoint, transitionEffectIn, transitionEffectOut));
    }
}
