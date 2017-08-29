﻿using UnityEngine;
using Tiled2Unity;
using System.Collections.Generic;

public enum Direction
{
    Right,
    Left,
    Top,
    Bottom,
    BottomLeft,
    BottomRight,
    TopLeft,
    TopRight,
}

public class Grid : MonoBehaviour
{

    public bool debugPathFinding = false;
    public TiledMap map;

    public int Width;
    public int Height;

    public Node[,] Nodes;

    public int Left { get { return 0; } }
    public int Right { get { return Width; } }
    public int Bottom { get { return 0; } }
    public int Top { get { return Height; } }

    public float heightBetweenPoints;
    public float widthBetweenPoints;

    GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player");

        map = GetComponent<TiledMap>();

        heightBetweenPoints = map.TileHeight;
        widthBetweenPoints = map.TileWidth;

        Width = (int)(((map.NumTilesWide * 2) * (map.TileWidth / widthBetweenPoints)) + 2);
        Height = (int)(((map.NumTilesHigh * 2) * (map.TileHeight / heightBetweenPoints)) + 2);

        Nodes = new Node[Width, Height];

        //Initialize the grid nodes - 1 grid unit between each node
        //We render the grid in a diamond pattern
        for (int x = 0; x < Width / 2; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float ptx = x * widthBetweenPoints;
                float pty = -(y * heightBetweenPoints / 2) + (heightBetweenPoints / 2f);
                int offsetx = 0;

                if (y % 2 == 0)
                {
                    ptx = x * widthBetweenPoints + (widthBetweenPoints / 2f);
                    offsetx = 1;
                }
                else
                {
                    pty = -(y * heightBetweenPoints / 2) + (heightBetweenPoints / 2f);
                }

                Nodes[x * 2 + offsetx, y] = new Node(x * 2 + offsetx, y, new Vector2(ptx, pty), this);
            }
        }

        //Create connections between each node
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Nodes[x, y] == null) continue;
                Nodes[x, y].initializeConnections(this);
            }
        }

        //Pass 1, we removed the bad nodes, based on valid connections
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Nodes[x, y] == null)
                    continue;

                Nodes[x, y].checkConnectionsPass1(this);
            }
        }

        //Pass 2, remove bad connections based on bad nodes
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Nodes[x, y] == null)
                    continue;

                Nodes[x, y].checkConnectionsPass2();
            }
        }
    }


    public Point worldToGrid(Vector2 worldPosition)
    {
        Vector2 gridPosition = new Vector2((worldPosition.x * 2f), -(worldPosition.y * 2f) + 1);

        //adjust to our nearest integer
        float rx = gridPosition.x % widthBetweenPoints;
        if (rx < (widthBetweenPoints / 2))
            gridPosition.x = gridPosition.x - rx;
        else
            gridPosition.x = gridPosition.x + (widthBetweenPoints - rx);

        float ry = gridPosition.y % heightBetweenPoints;
        if (ry < (heightBetweenPoints / 2))
            gridPosition.y = gridPosition.y - ry;
        else
            gridPosition.y = gridPosition.y + (heightBetweenPoints - ry);

        int x = (int)(gridPosition.x / widthBetweenPoints);
        int y = (int)(gridPosition.y / heightBetweenPoints);

        if (x < 0 || y < 0 || x > Width || y > Height)
            return null;

        Node node = Nodes[x, y];
        //We calculated a spot between nodes'
        //Find nearest neighbor
        if ((node == null) || (x % 2 == 0 && y % 2 == 0) || (gridPosition.y % 2 == 1 && gridPosition.x % 2 == 1))
        {
            float mag = 100;


            if (x < Width && !Nodes[x + 1, y].isBadNode)
            {
                float mag1 = (Nodes[x + 1, y].position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x + 1, y];
                }
            }
            if (y < Height - 1 && !Nodes[x, y + 1].isBadNode)
            {
                float mag1 = (Nodes[x, y + 1].position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x, y + 1];
                }
            }
            if (x > 0 && !Nodes[x - 1, y].isBadNode)
            {
                float mag1 = (Nodes[x - 1, y].position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x - 1, y];
                }
            }
            if (y > 0 && !Nodes[x, y - 1].isBadNode)
            {
                float mag1 = (Nodes[x, y - 1].position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x, y - 1 + 1];
                }
            }
        }
        return new Point(node.x, node.y);
    }

    public Vector2 gridToWorld(Point gridPosition)
    {
        Vector2 world = new Vector2((gridPosition.x / 2f) * widthBetweenPoints, (-(gridPosition.y / 2f - 0.5f)) * heightBetweenPoints);

        return world;
    }

    public bool isConnectionValid(Point point1, Point point2)
    {
        //comparing same point, return false
        if (point1.x == point2.x && point1.y == point2.y)
            return false;

        if (Nodes[point1.x, point1.y] == null)
            return false;

        //determine direction from point1 to point2
        Direction direction = Direction.Bottom;

        if (point1.x == point2.x)
        {
            if (point1.y < point2.y)
                direction = Direction.Bottom;
            else if (point1.y > point2.y)
                direction = Direction.Top;
        }
        else if (point1.y == point2.y)
        {
            if (point1.x < point2.x)
                direction = Direction.Right;
            else if (point1.x > point2.x)
                direction = Direction.Left;
        }
        else if (point1.x < point2.x)
        {
            if (point1.y > point2.y)
                direction = Direction.TopRight;
            else if (point1.y < point2.y)
                direction = Direction.BottomRight;
        }
        else if (point1.x > point2.x)
        {
            if (point1.y > point2.y)
                direction = Direction.TopLeft;
            else if (point1.y < point2.y)
                direction = Direction.BottomLeft;
        }

        //check connection
        switch (direction)
        {
            case Direction.Bottom:
                if (Nodes[point1.x, point1.y].nodeAtBottom != null)
                    return Nodes[point1.x, point1.y].nodeAtBottom.isValid;
                else
                    return false;

            case Direction.Top:
                if (Nodes[point1.x, point1.y].nodeAtTop != null)
                    return Nodes[point1.x, point1.y].nodeAtTop.isValid;
                else
                    return false;

            case Direction.Right:
                if (Nodes[point1.x, point1.y].nodeAtRight != null)
                    return Nodes[point1.x, point1.y].nodeAtRight.isValid;
                else
                    return false;

            case Direction.Left:
                if (Nodes[point1.x, point1.y].nodeAtLeft != null)
                    return Nodes[point1.x, point1.y].nodeAtLeft.isValid;
                else
                    return false;

            case Direction.BottomLeft:
                if (Nodes[point1.x, point1.y].nodeAtBottomLeft != null)
                    return Nodes[point1.x, point1.y].nodeAtBottomLeft.isValid;
                else
                    return false;

            case Direction.BottomRight:
                if (Nodes[point1.x, point1.y].nodeAtBottomRight != null)
                    return Nodes[point1.x, point1.y].nodeAtBottomRight.isValid;
                else
                    return false;

            case Direction.TopLeft:
                if (Nodes[point1.x, point1.y].nodeAtTopLeft != null)
                    return Nodes[point1.x, point1.y].nodeAtTopLeft.isValid;
                else
                    return false;

            case Direction.TopRight:
                if (Nodes[point1.x, point1.y].nodeAtTopRight != null)
                    return Nodes[point1.x, point1.y].nodeAtTopRight.isValid;
                else
                    return false;

            default:
                return false;
        }
    }


    void Update()
    {

        if (debugPathFinding && Input.GetMouseButtonDown(1))
        {
            //Convert mouse click point to grid coordinates
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Point gridPos = worldToGrid(worldPos);

            if (gridPos != null)
            {

                if (gridPos.x > 0 && gridPos.y > 0 && gridPos.x < Width && gridPos.y < Height)
                {

                    //Convert player point to grid coordinates
                    Point playerPos = worldToGrid(player.transform.position);
                    if (debugPathFinding)
                    {
                        Nodes[playerPos.x, playerPos.y].setColor(Color.blue);
                    }

                    //Find path from player to clicked position
                    BreadCrumb bc = PathFinder.findPath(this, playerPos, gridPos);

                    //  Draw line
                    LineRenderer lr = player.GetComponent<LineRenderer>();
                    lr.positionCount = 100;
                    lr.startWidth = 1;
                    lr.endWidth = 1;
                    lr.startColor = Color.yellow;
                    lr.endColor = Color.yellow;
                    int count = 0;
                    //Draw out our path
                    while (bc != null)
                    {
                        lr.SetPosition(count, gridToWorld(bc.position));
                        bc = bc.next;
                        count += 1;
                    }
                    lr.positionCount = count;

                    // Move player
                    List<Vector2> points = new List<Vector2>();
                    BreadCrumb breadcrumb = PathFinder.findPath(this, playerPos, gridPos);
                    while (breadcrumb != null)
                    {
                        points.Add(gridToWorld(breadcrumb.position));
                        breadcrumb = breadcrumb.next;
                    }
                    if (points.Count > 0)
                    {
                        player.GetComponent<Player>().moveToPosition(points);
                    }

                }
            }
        }
    }

}



