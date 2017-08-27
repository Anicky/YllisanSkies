using UnityEngine;

public class MoveBackground : MonoBehaviour
{

    public float scrollSpeedX;
    public float scrollSpeedY;

    private float count;

    private Game game;

    // Use this for initialization
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!game.stopEvents)
        {
            Vector2 offset = new Vector2(count * (scrollSpeedX / 100), count * (scrollSpeedY / 100));
            Renderer rend = GetComponent<Renderer>();
            rend.material.mainTextureOffset = offset;
            count += 0.01f;
        }
    }
}
