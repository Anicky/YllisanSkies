using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadMap : MonoBehaviour
{

    public string mapToLoad;
    public string startingPoint;
    private bool isTriggered = false;
    public bool needsSubmitButton = false;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if ((isTriggered) && ((!needsSubmitButton) || (needsSubmitButton && Input.GetButton("Submit"))))
        {
            isTriggered = false;
            var thePlayer = FindObjectOfType<Player>();
            thePlayer.eventNameWherePlayerHasToBeTeleported = startingPoint;
            StartCoroutine(fadeIn());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isTriggered = false;
        }
    }

    private IEnumerator fadeIn()
    {
        GameObject game = GameObject.Find("Game");
        Animation anim = game.GetComponent<Animation>();
        anim.Play("Overlay_FadeIn");
        do
        {
            yield return null;
        } while (anim.isPlaying);
        SceneManager.LoadScene(mapToLoad);
        StartCoroutine(fadeOut());
    }

    private IEnumerator fadeOut()
    {
        GameObject game = GameObject.Find("Game");
        Animation anim = game.GetComponent<Animation>();
        anim.Play("Overlay_FadeOut");
        do
        {
            yield return null;
        } while (anim.isPlaying);
    }

}
