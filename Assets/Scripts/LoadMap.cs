using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadMap : MonoBehaviour
{

    public string mapToLoad;
    public string startingPoint;
    private bool isTriggered = false;
    public bool submitButtonNeeded = false;
    public Vector2 playerDirectionNeeded;
    private Player player;
    private bool inTransition = false;

    // Use this for initialization
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isTriggered && checkSubmitButton() && checkDirection())
        {
            isTriggered = false;
            StartCoroutine(changeScene());
        }
    }

    private bool checkSubmitButton()
    {
        return (!submitButtonNeeded || (submitButtonNeeded && Input.GetButton("Submit")));
    }

    private bool checkDirection()
    {
        if (playerDirectionNeeded == new Vector2(0, 0))
        {
            return true;
        }
        else if ((playerDirectionNeeded.x != 0) && (player.lastMove.x == playerDirectionNeeded.x))
        {
            return true;
        }
        else if ((playerDirectionNeeded.y != 0) && (player.lastMove.y == playerDirectionNeeded.y))
        {
            return true;
        }
        return false;
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

    private IEnumerator changeScene()
    {
        StartCoroutine(transition("Overlay_FadeIn"));
        do
        {
            yield return null;
        } while (inTransition);
        SceneManager.LoadScene(mapToLoad);
        player.eventNameWherePlayerHasToBeTeleported = startingPoint;
        StartCoroutine(transition("Overlay_FadeOut"));
    }

    private IEnumerator transition(string transition)
    {
        inTransition = true;
        GameObject game = GameObject.Find("Game");
        Animation anim = game.GetComponent<Animation>();
        anim.Play(transition);
        do
        {
            yield return null;
        } while (anim.isPlaying);
        inTransition = false;
    }

}
