using UnityEngine;

public abstract class Event : MonoBehaviour
{

    protected bool isTriggered = false;
    public bool submitButtonNeeded = false;
    public Vector2 playerDirectionNeeded;
    protected Player player;
    protected Game game;

    // Use this for initialization
    protected void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isTriggered && checkSubmitButton() && checkDirection())
        {
            isTriggered = false;
            doActionWhenTriggered();
        }
    }

    protected abstract void doActionWhenTriggered();

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

}
