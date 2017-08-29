using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    private Rigidbody2D rbody;
    private Animator anim;
    public float speed = 64;
    private Tiled2Unity.TiledMap map;
    public Vector2 lastMove;
    private bool movementEnabled = true;
    private bool firstInit = true;
    public bool isMovingToPosition = false;
    private List<Vector2> pointsToMove;
    private Vector2 nextPointToMove;
    private bool arrivedToNextPoint = false;

    private float getRelativeX()
    {
        return rbody.position.x - map.transform.position.x;
    }

    private float getRelativeY()
    {
        return rbody.position.y - map.transform.position.y;
    }

    // Use this for initialization
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void initialize()
    {
        GameObject mapObject = GameObject.Find("Map");
        if (mapObject)
        {
            map = mapObject.GetComponentInParent<Tiled2Unity.TiledMap>();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        initialize();
        if (firstInit)
        {
            if (map)
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }
            GameObject playerStartingPoint = GameObject.Find("PlayerStartingPoint");
            if (playerStartingPoint != null)
            {
                transform.position = playerStartingPoint.transform.position;
            }
            firstInit = false;
        }
    }

    private void checkBypassCollisions()
    {
        if (Input.GetButtonDown("Bypass Collisions"))
        {
            Debug.Log("Collisions bypassed");
            Physics2D.IgnoreLayerCollision(0, 0);
        }
        if (Input.GetButtonUp("Bypass Collisions"))
        {
            Debug.Log("Collisions back to normal");
            Physics2D.SetLayerCollisionMask(0, 63);
        }
    }

    private void checkMovement()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }

    private void move(Vector2 movementVector)
    {
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
            if (movementVector.x > 0 && getRelativeX() > map.GetMapWidthInPixelsScaled() - 12)
            {
                isWalking = false;
            }
            if (movementVector.y < 0 && getRelativeY() < -map.GetMapHeightInPixelsScaled() + 16)
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


    // Update is called once per frame
    private void Update()
    {
        if (map)
        {
            if (isMovingToPosition)
            {
                prepareNextMove();
            }
            else
            {
                checkBypassCollisions();
                if (movementEnabled)
                {
                    checkMovement();
                }
            }
        }
    }

    public void disableMovement()
    {
        movementEnabled = false;
        anim.SetBool("is_walking", false);
    }

    public void enableMovement()
    {
        movementEnabled = true;
    }

    public void moveToPosition(List<Vector2> points)
    {
        pointsToMove = points;
        nextPointToMove = pointsToMove[0];
        pointsToMove.RemoveAt(0);
        arrivedToNextPoint = false;
        isMovingToPosition = true;
    }

    private void prepareNextMove()
    {
        if (!arrivedToNextPoint)
        {
            int targetPositionX = (int)nextPointToMove.x;
            int targetPositionY = (int)nextPointToMove.y;
            int currentPositionX = (int)transform.position.x;
            int currentPositionY = (int)transform.position.y;
            if ((currentPositionX > targetPositionX - 2) && (currentPositionX < targetPositionX + 2) && (currentPositionY > targetPositionY - 2) && (currentPositionY < targetPositionY + 2))
            {
                arrivedToNextPoint = true;
            }
            else
            {
                int movementVectorX = 0;
                int movementVectorY = 0;
                if (currentPositionX < targetPositionX)
                {
                    movementVectorX = 1;
                }
                else if (currentPositionX > targetPositionX)
                {
                    movementVectorX = -1;
                }
                if (currentPositionY < targetPositionY)
                {
                    movementVectorY = 1;
                }
                else if (currentPositionY > targetPositionY)
                {
                    movementVectorY = -1;
                }
                move(new Vector2(movementVectorX, movementVectorY));
            }
        } else if (pointsToMove.Count > 0)
        {
            nextPointToMove = pointsToMove[0];
            pointsToMove.RemoveAt(0);
            arrivedToNextPoint = false;
        } else
        {
            isMovingToPosition = false;
        }


    }
}
