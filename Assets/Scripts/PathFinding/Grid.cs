using UnityEngine;
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

    private LineRenderer LineRenderer;
    GameObject Player;

    void Awake()
    {
        Player = GameObject.Find("Player");
        LineRenderer = transform.GetComponent<LineRenderer>();

        map = GetComponent<TiledMap>();

        heightBetweenPoints = map.TileHeight / 2;
        widthBetweenPoints = map.TileWidth / 2;

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
                Nodes[x, y].InitializeConnections(this);
            }
        }

        //Pass 1, we removed the bad nodes, based on valid connections
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Nodes[x, y] == null)
                    continue;

                Nodes[x, y].CheckConnectionsPass1(this);
            }
        }

        //Pass 2, remove bad connections based on bad nodes
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Nodes[x, y] == null)
                    continue;

                Nodes[x, y].CheckConnectionsPass2();
            }
        }
    }


    public Point WorldToGrid(Vector2 worldPosition)
    {
        Vector2 gridPosition = new Vector2((worldPosition.x * 2f), -(worldPosition.y * 2f) + 1);

        //adjust to our nearest integer
        float rx = gridPosition.x % widthBetweenPoints;
        if (rx < 8f)
            gridPosition.x = gridPosition.x - rx;
        else
            gridPosition.x = gridPosition.x + (widthBetweenPoints - rx);

        float ry = gridPosition.y % heightBetweenPoints;
        if (ry < 8f)
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


            if (x < Width && !Nodes[x + 1, y].BadNode)
            {
                float mag1 = (Nodes[x + 1, y].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x + 1, y];
                }
            }
            if (y < Height - 1 && !Nodes[x, y + 1].BadNode)
            {
                float mag1 = (Nodes[x, y + 1].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x, y + 1];
                }
            }
            if (x > 0 && !Nodes[x - 1, y].BadNode)
            {
                float mag1 = (Nodes[x - 1, y].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x - 1, y];
                }
            }
            if (y > 0 && !Nodes[x, y - 1].BadNode)
            {
                float mag1 = (Nodes[x, y - 1].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x, y - 1 + 1];
                }
            }
        }
        return new Point(node.X, node.Y);
    }

    public Vector2 GridToWorld(Point gridPosition)
    {
        Vector2 world = new Vector2((gridPosition.X / 2f) * widthBetweenPoints, (-(gridPosition.Y / 2f - 0.5f)) * heightBetweenPoints);

        return world;
    }

    public bool ConnectionIsValid(Point point1, Point point2)
    {
        //comparing same point, return false
        if (point1.X == point2.X && point1.Y == point2.Y)
            return false;

        if (Nodes[point1.X, point1.Y] == null)
            return false;

        //determine direction from point1 to point2
        Direction direction = Direction.Bottom;

        if (point1.X == point2.X)
        {
            if (point1.Y < point2.Y)
                direction = Direction.Bottom;
            else if (point1.Y > point2.Y)
                direction = Direction.Top;
        }
        else if (point1.Y == point2.Y)
        {
            if (point1.X < point2.X)
                direction = Direction.Right;
            else if (point1.X > point2.X)
                direction = Direction.Left;
        }
        else if (point1.X < point2.X)
        {
            if (point1.Y > point2.Y)
                direction = Direction.TopRight;
            else if (point1.Y < point2.Y)
                direction = Direction.BottomRight;
        }
        else if (point1.X > point2.X)
        {
            if (point1.Y > point2.Y)
                direction = Direction.TopLeft;
            else if (point1.Y < point2.Y)
                direction = Direction.BottomLeft;
        }

        //check connection
        switch (direction)
        {
            case Direction.Bottom:
                if (Nodes[point1.X, point1.Y].Bottom != null)
                    return Nodes[point1.X, point1.Y].Bottom.Valid;
                else
                    return false;

            case Direction.Top:
                if (Nodes[point1.X, point1.Y].Top != null)
                    return Nodes[point1.X, point1.Y].Top.Valid;
                else
                    return false;

            case Direction.Right:
                if (Nodes[point1.X, point1.Y].Right != null)
                    return Nodes[point1.X, point1.Y].Right.Valid;
                else
                    return false;

            case Direction.Left:
                if (Nodes[point1.X, point1.Y].Left != null)
                    return Nodes[point1.X, point1.Y].Left.Valid;
                else
                    return false;

            case Direction.BottomLeft:
                if (Nodes[point1.X, point1.Y].BottomLeft != null)
                    return Nodes[point1.X, point1.Y].BottomLeft.Valid;
                else
                    return false;

            case Direction.BottomRight:
                if (Nodes[point1.X, point1.Y].BottomRight != null)
                    return Nodes[point1.X, point1.Y].BottomRight.Valid;
                else
                    return false;

            case Direction.TopLeft:
                if (Nodes[point1.X, point1.Y].TopLeft != null)
                    return Nodes[point1.X, point1.Y].TopLeft.Valid;
                else
                    return false;

            case Direction.TopRight:
                if (Nodes[point1.X, point1.Y].TopRight != null)
                    return Nodes[point1.X, point1.Y].TopRight.Valid;
                else
                    return false;

            default:
                return false;
        }
    }


    void Update()
    {
        //Pathfinding demo
        if (Input.GetMouseButtonDown(0))
        {
            //Convert mouse click point to grid coordinates
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Point gridPos = WorldToGrid(worldPos);

            if (gridPos != null)
            {

                if (gridPos.X > 0 && gridPos.Y > 0 && gridPos.X < Width && gridPos.Y < Height)
                {

                    //Convert player point to grid coordinates
                    Point playerPos = WorldToGrid(Player.transform.position);
                    if (debugPathFinding)
                    {
                        Nodes[playerPos.X, playerPos.Y].SetColor(Color.blue);
                    }

                    //Find path from player to clicked position
                    BreadCrumb bc = PathFinder.FindPath(this, playerPos, gridPos);

                    ////////////////////////////// DRAW LINE

                    LineRenderer lr = Player.GetComponent<LineRenderer>();
                    lr.SetVertexCount(100);  //Need a higher number than 2, or crashes out
                    lr.SetWidth(4f, 4f);
                    lr.SetColors(Color.yellow, Color.yellow);
                    int count = 0;
                    //Draw out our path
                    while (bc != null)
                    {
                        lr.SetPosition(count, GridToWorld(bc.position));
                        bc = bc.next;
                        count += 1;
                    }
                    lr.SetVertexCount(count);

                    ////////////////////////////////// END


                    List<Vector2> points = new List<Vector2>();
                    BreadCrumb breadcrumb = PathFinder.FindPath(this, playerPos, gridPos);
                    while (breadcrumb != null)
                    {
                        points.Add(GridToWorld(breadcrumb.position));
                        breadcrumb = breadcrumb.next;
                    }
                    if (points.Count > 0)
                    {
                        Player.GetComponent<Player>().moveToPosition(points);
                    }

                }
            }
        }
    }

}



