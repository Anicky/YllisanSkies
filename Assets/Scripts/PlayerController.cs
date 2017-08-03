using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody;
    Animator anim;
    public float speed = 64;
    Tiled2Unity.TiledMap tiledMap;
    private static bool playerExists;
    public Vector2 lastMove;

    float getRelativeX()
    {
        return rbody.position.x - tiledMap.transform.position.x;
    }

    float getRelativeY()
    {
        return rbody.position.y - tiledMap.transform.position.y;
    }

    // Use this for initialization
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        initialize();
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void initialize()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject map = GameObject.Find("Map");
        tiledMap = map.GetComponentInParent<Tiled2Unity.TiledMap>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        bool isWalking = false;

        if (movementVector != Vector2.zero)
        {
            isWalking = true;
            if (movementVector.x < 0 && getRelativeX() < 8)
            {
                isWalking = false;
            }
            if (movementVector.y > 0 && getRelativeY() > 0)
            {
                isWalking = false;
            }
            if (movementVector.x > 0 && getRelativeX() > tiledMap.GetMapWidthInPixelsScaled() - 12)
            {
                isWalking = false;
            }
            if (movementVector.y < 0 && getRelativeY() < -tiledMap.GetMapHeightInPixelsScaled() + 16)
            {
                isWalking = false;
            }
        }

        if (isWalking)
        {
            anim.SetBool("is_walking", true);
            anim.SetFloat("input_x", movementVector.x);
            anim.SetFloat("input_y", movementVector.y);
            rbody.MovePosition(rbody.position + speed * (movementVector * Time.deltaTime));
            lastMove = movementVector;
        }
        else
        {
            anim.SetBool("is_walking", false);
        }
    }
}
