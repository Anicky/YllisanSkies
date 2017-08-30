﻿using UnityEngine;
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
    private Node initialNodeBeforeMovement;
    private Grid grid;
    private LineRenderer lineRenderer;
    private List<Node> nodesToDrawForDirectPath;

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
        GameObject gameObject = GameObject.Find("Game");
        if (gameObject)
        {
            game = gameObject.GetComponentInParent<Game>();
        }
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void initialize()
    {
        GameObject mapObject = GameObject.Find("Map");
        if (mapObject)
        {
            map = mapObject.GetComponentInParent<Tiled2Unity.TiledMap>();
            grid = map.GetComponent<Grid>();
            pointsToMove = new List<Vector2>();
            nodesToDrawForDirectPath = new List<Node>();
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
    private void FixedUpdate()
    {
        if (map && !game.stopEvents)
        {
            if (game.debugMode)
            {
                checkBypassCollisions();
                if (Input.GetMouseButtonDown(1))
                {
                    if (!isMovingToPosition && pointsToMove.Count > 0)
                    {
                        grid.showPathLines = true;
                        initLineRenderer(false);
                        Point playerPos = grid.worldToGrid(transform.position);
                        int count = 0;
                        Vector2 startingPoint = grid.gridToWorld(playerPos);
                        lineRenderer.SetPosition(count, startingPoint);
                        count += 1;
                        foreach (Vector2 point in pointsToMove)
                        {
                            lineRenderer.SetPosition(count, point);
                            count += 1;
                        }
                        lineRenderer.positionCount = count;
                        initMovement();
                    }
                    else
                    {
                        grid.showPathLines = true;
                        Vector2 mousePosition = getPositionFromMouse();
                        Debug.Log(mousePosition);
                        moveToPosition(mousePosition);
                    }
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePosition = getPositionFromMouse();
                    Debug.Log(mousePosition);
                    Point gridPos = grid.worldToGrid(mousePosition);
                    Vector2 nodePos = grid.gridToWorld(gridPos);
                    Node node = new Node(gridPos.x, gridPos.y, nodePos, grid);
                    node.draw(true);
                    nodesToDrawForDirectPath.Add(node);
                    pointsToMove.Add(nodePos);
                }
            }
            if (isMovingToPosition)
            {
                prepareNextMove();
            }
            else if (movementEnabled)
            {
                checkMovement();
            }
        }
    }

    private Vector2 getPositionFromMouse()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

    private void initLineRenderer(bool isPathFinding = true)
    {
        lineRenderer.positionCount = 100;
        lineRenderer.startWidth = 1;
        lineRenderer.endWidth = 1;
        if (isPathFinding)
        {
            lineRenderer.startColor = Grid.colorPathFindingWalkable;
            lineRenderer.endColor = Grid.colorPathFindingWalkable;
        }
        else
        {
            lineRenderer.startColor = Grid.colorDirectPath;
            lineRenderer.endColor = Grid.colorDirectPath;
        }
    }

    public void moveToPosition(Vector2 position, bool usePathfinding = true)
    {
        if (grid.debugMode && initialNodeBeforeMovement != null)
        {
            initialNodeBeforeMovement.setColor(Grid.colorPathFindingWalkable);
        }
        Point gridPos = grid.worldToGrid(position);
        if (gridPos != null)
        {
            if (gridPos.x > 0 && gridPos.y > 0 && gridPos.x < grid.Width && gridPos.y < grid.Height)
            {
                Point playerPos = grid.worldToGrid(transform.position);
                int count = 0;

                if (grid.debugMode)
                {
                    initialNodeBeforeMovement = grid.Nodes[playerPos.x, playerPos.y];
                    initialNodeBeforeMovement.setColor(Color.blue);
                }
                if (grid.showPathLines)
                {
                    initLineRenderer();
                }

                pointsToMove = new List<Vector2>();

                if (usePathfinding)
                {
                    BreadCrumb breadcrumb = PathFinder.findPath(grid, playerPos, gridPos);
                    while (breadcrumb != null)
                    {
                        Vector2 point = grid.gridToWorld(breadcrumb.position);
                        pointsToMove.Add(point);
                        if (grid.showPathLines)
                        {
                            lineRenderer.SetPosition(count, point);
                            count += 1;
                        }
                        breadcrumb = breadcrumb.next;
                    }
                }
                else
                {
                    Vector2 point = grid.gridToWorld(gridPos);
                    pointsToMove.Add(point);
                    if (grid.showPathLines)
                    {
                        Vector2 startingPoint = grid.gridToWorld(playerPos);
                        lineRenderer.SetPosition(count, startingPoint);
                        count += 1;
                        lineRenderer.SetPosition(count, point);
                        count += 1;
                    }
                }

                if (grid.showPathLines)
                {
                    lineRenderer.positionCount = count;
                }

                initMovement();
            }
        }
    }

    private void initMovement()
    {
        if (pointsToMove.Count > 0)
        {
            nextPointToMove = pointsToMove[0];
            pointsToMove.RemoveAt(0);
            arrivedToNextPoint = false;
            isMovingToPosition = true;
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
                    float distanceForThisFrame = Mathf.Min(distanceToNextPoint, numberOfPixelsToMoveLeftForThisFrame);
                    move(new Vector2(movementVectorX, movementVectorY), distanceForThisFrame);
                    numberOfPixelsToMoveLeftForThisFrame -= distanceForThisFrame;
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
                if (grid.showPathLines)
                {
                    lineRenderer.positionCount = 0;
                    foreach (Node node in nodesToDrawForDirectPath)
                    {
                        Destroy(node.nodeDebug);
                    }
                    nodesToDrawForDirectPath = new List<Node>();
                }
                if (grid.debugMode)
                {
                    initialNodeBeforeMovement.setColor(Grid.colorPathFindingWalkable);
                }
                grid.showPathLines = false;
            }
        }
    }
}
