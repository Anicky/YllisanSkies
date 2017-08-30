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
    private Game game;

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
        GameObject gameObject = GameObject.Find("Game");
        if (gameObject)
        {
            game = gameObject.GetComponentInParent<Game>();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        initialize();
        if (firstInit)
        {
            if (this && map)
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }
            GameObject playerStartingPoint = GameObject.Find("PlayerStartingPoint");
            if (this && playerStartingPoint != null)
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
        move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), numberOfPixelsToMoveInOneFrame());
    }

    private void move(Vector2 movementVector, float numberOfPixelsToMove)
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
            rbody.MovePosition(rbody.position + numberOfPixelsToMove * movementVector);
            lastMove = movementVector;
        }
        else
        {
            anim.SetBool("is_walking", false);
        }
    }

    private float numberOfPixelsToMoveInOneFrame()
    {
        return speed * Time.deltaTime;
    }


    // Update is called once per frame
    private void Update()
    {
        if (game && game.debugMode && Input.GetMouseButtonDown(1))
        {
            moveToPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (map && !game.stopEvents)
        {
            if (isMovingToPosition)
            {
                prepareNextMove();
            }
            else if (game.debugMode)
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

    public void moveToPosition(Vector2 position, bool usePathfinding = true)
    {
        Grid grid = map.GetComponent<Grid>();
        Point gridPos = grid.worldToGrid(position);
        if (gridPos != null)
        {
            if (gridPos.x > 0 && gridPos.y > 0 && gridPos.x < grid.Width && gridPos.y < grid.Height)
            {
                Point playerPos = grid.worldToGrid(transform.position);
                int count = 0;
                LineRenderer lr = GetComponent<LineRenderer>();

                if (game.debugMode)
                {
                    if (grid.debugPathFinding)
                    {
                        grid.Nodes[playerPos.x, playerPos.y].setColor(Color.blue);
                    }
                    lr.positionCount = 100;
                    lr.startWidth = 1;
                    lr.endWidth = 1;
                    lr.startColor = Color.yellow;
                    lr.endColor = Color.yellow;
                }

                pointsToMove = new List<Vector2>();

                if (usePathfinding)
                {
                    BreadCrumb breadcrumb = PathFinder.findPath(grid, playerPos, gridPos);
                    while (breadcrumb != null)
                    {
                        Vector2 point = grid.gridToWorld(breadcrumb.position);
                        pointsToMove.Add(point);
                        if (game.debugMode)
                        {
                            lr.SetPosition(count, point);
                            count += 1;
                        }
                        breadcrumb = breadcrumb.next;
                    }
                }
                else
                {
                    Vector2 point = grid.gridToWorld(gridPos);
                    pointsToMove.Add(point);
                    if (game.debugMode)
                    {
                        Vector2 startingPoint = grid.gridToWorld(playerPos);
                        lr.SetPosition(count, startingPoint);
                        count += 1;
                        lr.SetPosition(count, point);
                        count += 1;
                    }
                }

                if (game.debugMode)
                {
                    lr.positionCount = count;
                }

                if (pointsToMove.Count > 0)
                {
                    nextPointToMove = pointsToMove[0];
                    pointsToMove.RemoveAt(0);
                    arrivedToNextPoint = false;
                    isMovingToPosition = true;
                }
            }
        }
    }

    private void prepareNextMove()
    {
        float numberOfPixelsToMoveLeftForThisFrame = numberOfPixelsToMoveInOneFrame();
        while (numberOfPixelsToMoveLeftForThisFrame > 0)
        {
            if (!arrivedToNextPoint)
            {
                int targetPositionX = (int)nextPointToMove.x;
                int targetPositionY = (int)nextPointToMove.y;
                int currentPositionX = (int)transform.position.x;
                int currentPositionY = (int)transform.position.y;
                if ((currentPositionX >= targetPositionX - 1) && (currentPositionX <= targetPositionX + 1) && (currentPositionY >= targetPositionY - 1) && (currentPositionY <= targetPositionY + 1))
                {
                    arrivedToNextPoint = true;
                }
                else
                {
                    int movementVectorX = 0;
                    int movementVectorY = 0;
                    if (currentPositionX < targetPositionX - 1)
                    {
                        movementVectorX = 1;
                    }
                    else if (currentPositionX > targetPositionX + 1)
                    {
                        movementVectorX = -1;
                    }
                    if (currentPositionY < targetPositionY - 1)
                    {
                        movementVectorY = 1;
                    }
                    else if (currentPositionY > targetPositionY + 1)
                    {
                        movementVectorY = -1;
                    }

                    float distanceToNextPoint = Vector2.Distance(new Vector2(currentPositionX, currentPositionY), new Vector2(targetPositionX, targetPositionY));

                    if (distanceToNextPoint > numberOfPixelsToMoveLeftForThisFrame)
                    {
                        move(new Vector2(movementVectorX, movementVectorY), numberOfPixelsToMoveLeftForThisFrame);
                        numberOfPixelsToMoveLeftForThisFrame = 0;
                    }
                    else
                    {
                        move(new Vector2(movementVectorX, movementVectorY), distanceToNextPoint);
                        numberOfPixelsToMoveLeftForThisFrame -= distanceToNextPoint;
                    }
                }
            }
            else if (pointsToMove.Count > 0)
            {
                nextPointToMove = pointsToMove[0];
                pointsToMove.RemoveAt(0);
                arrivedToNextPoint = false;
            }
            else
            {
                numberOfPixelsToMoveLeftForThisFrame = 0;
                isMovingToPosition = false;
            }
        }
    }
}
