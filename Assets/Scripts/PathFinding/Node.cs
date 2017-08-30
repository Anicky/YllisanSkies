using UnityEngine;

public class Node
{

    public bool isBadNode;

    //Grid coordinates
    public int x;
    public int y;

    //world position
    public Vector2 position;

    //our 8 connection points
    public NodeConnection nodeAtTopLeft;
    public NodeConnection nodeAtTop;
    public NodeConnection nodeAtTopRight;
    public NodeConnection nodeAtLeft;
    public NodeConnection nodeAtRight;
    public NodeConnection nodeAtBottomLeft;
    public NodeConnection nodeAtBottom;
    public NodeConnection nodeAtBottomRight;

    //debug
    public GameObject nodeDebug;

    public Node(float x, float y, Vector2 position, Grid grid)
    {
        initialize(x, y, position, grid);
    }

    public void initialize(float x, float y, Vector2 position, Grid grid)
    {
        this.x = (int)x;
        this.y = (int)y;

        this.position = position;

        //check if coords inside our grid area
        if (this.position.x < grid.transform.position.x || -this.position.y < grid.transform.position.y)
        {
            disableConnections();
            isBadNode = true;
        }
        if (this.position.x > grid.transform.position.x + grid.map.MapWidthInPixels)
        {
            disableConnections();
            isBadNode = true;
        }
        if (-this.position.y > grid.transform.position.y + grid.map.MapHeightInPixels)
        {
            disableConnections();
            isBadNode = true;
        }

        if (grid.debugMode)
        {
            draw();
        }
    }

    public void draw(bool isDirectPath = false)
    {
        nodeDebug = GameObject.Instantiate(Resources.Load("Pathfinding/Node")) as GameObject;
        nodeDebug.transform.position = this.position;
        if (isDirectPath)
        {
            setColor(Grid.colorDirectPath);
        }
    }

    public void setColor(Color color)
    {
        nodeDebug.transform.GetComponent<SpriteRenderer>().color = color;
    }

    //Cull nodes if they don't have enough valid connection points (3)
    public void checkConnectionsPass1()
    {
        if (!isBadNode)
        {

            int clearCount = 0;

            if (nodeAtTop != null && nodeAtTop.isValid)
                clearCount++;
            if (nodeAtBottom != null && nodeAtBottom.isValid)
                clearCount++;
            if (nodeAtLeft != null && nodeAtLeft.isValid)
                clearCount++;
            if (nodeAtRight != null && nodeAtRight.isValid)
                clearCount++;
            if (nodeAtTopLeft != null && nodeAtTopLeft.isValid)
                clearCount++;
            if (nodeAtTopRight != null && nodeAtTopRight.isValid)
                clearCount++;
            if (nodeAtBottomLeft != null && nodeAtBottomLeft.isValid)
                clearCount++;
            if (nodeAtBottomRight != null && nodeAtBottomRight.isValid)
                clearCount++;

            //If not at least 1 valid connection point - disable node
            if (clearCount < 1)
            {
                isBadNode = true;
                disableConnections();
            }
        }
    }

    //Remove connections that connect to bad nodes
    public void checkConnectionsPass2()
    {
        if (nodeAtTop != null && nodeAtTop.node != null && nodeAtTop.node.isBadNode)
            nodeAtTop.isValid = false;
        if (nodeAtBottom != null && nodeAtBottom.node != null && nodeAtBottom.node.isBadNode)
            nodeAtBottom.isValid = false;
        if (nodeAtLeft != null && nodeAtLeft.node != null && nodeAtLeft.node.isBadNode)
            nodeAtLeft.isValid = false;
        if (nodeAtRight != null && nodeAtRight.node != null && nodeAtRight.node.isBadNode)
            nodeAtRight.isValid = false;
        if (nodeAtTopLeft != null && nodeAtTopLeft.node != null && nodeAtTopLeft.node.isBadNode)
            nodeAtTopLeft.isValid = false;
        if (nodeAtTopRight != null && nodeAtTopRight.node != null && nodeAtTopRight.node.isBadNode)
            nodeAtTopRight.isValid = false;
        if (nodeAtBottomLeft != null && nodeAtBottomLeft.node != null && nodeAtBottomLeft.node.isBadNode)
            nodeAtBottomLeft.isValid = false;
        if (nodeAtBottomRight != null && nodeAtBottomRight.node != null && nodeAtBottomRight.node.isBadNode)
            nodeAtBottomRight.isValid = false;
    }

    //Disable all connections going from this this
    public void disableConnections()
    {
        if (nodeAtTop != null)
        {
            nodeAtTop.isValid = false;
        }
        if (nodeAtBottom != null)
        {
            nodeAtBottom.isValid = false;
        }
        if (nodeAtLeft != null)
        {
            nodeAtLeft.isValid = false;
        }
        if (nodeAtRight != null)
        {
            nodeAtRight.isValid = false;
        }
        if (nodeAtBottomLeft != null)
        {
            nodeAtBottomLeft.isValid = false;
        }
        if (nodeAtBottomRight != null)
        {
            nodeAtBottomRight.isValid = false;
        }
        if (nodeAtTopRight != null)
        {
            nodeAtTopRight.isValid = false;
        }
        if (nodeAtTopLeft != null)
        {
            nodeAtTopLeft.isValid = false;
        }
    }

    //Raycast in all 8 directions to determine valid routes
    public void initializeConnections(Grid grid)
    {
        bool valid = true;
        RaycastHit2D hit;
        float diagonalDistance = Mathf.Sqrt(Mathf.Pow(grid.widthBetweenPoints / 2f, 2) + Mathf.Pow(grid.heightBetweenPoints / 2f, 2));

        if (x > 1)
        {
            //Left
            valid = true;
            hit = Physics2D.Raycast(position, new Vector2(-1, 0), grid.widthBetweenPoints);
            if (hit.collider != null && !hit.collider.isTrigger && hit.collider.tag != "Player")
            {
                valid = false;
            }
            nodeAtLeft = new NodeConnection(this, grid.Nodes[x - 2, y], valid);

            //TopLeft
            if (y > 0)
            {
                valid = true;
                hit = Physics2D.Raycast(position, new Vector2(-1, 1), diagonalDistance);
                if (hit.collider != null && !hit.collider.isTrigger && hit.collider.tag != "Player")
                {
                    valid = false;
                }
                nodeAtTopLeft = new NodeConnection(this, grid.Nodes[x - 1, y - 1], valid);
            }

            //BottomLeft
            if (y < grid.Height - 1)
            {
                valid = true;
                hit = Physics2D.Raycast(position, new Vector2(-1, -1), diagonalDistance);
                if (hit.collider != null && !hit.collider.isTrigger && hit.collider.tag != "Player")
                {
                    valid = false;
                }
                nodeAtBottomLeft = new NodeConnection(this, grid.Nodes[x - 1, y + 1], valid);
            }
        }


        if (x < grid.Width - 2)
        {
            valid = true;
            hit = Physics2D.Raycast(position, new Vector2(1, 0), grid.widthBetweenPoints);
            if (hit.collider != null && !hit.collider.isTrigger && hit.collider.tag != "Player")
            {
                valid = false;
            }
            nodeAtRight = new NodeConnection(this, grid.Nodes[x + 2, y], valid);

            //TopRight
            if (y > 0)
            {
                valid = true;
                hit = Physics2D.Raycast(position, new Vector2(1, 1), diagonalDistance);
                if (hit.collider != null && !hit.collider.isTrigger && hit.collider.tag != "Player")
                {
                    valid = false;
                }
                nodeAtTopRight = new NodeConnection(this, grid.Nodes[x + 1, y - 1], valid);
            }

            //BottomRight
            if (y < grid.Height - 1)
            {
                valid = true;
                hit = Physics2D.Raycast(position, new Vector2(1, -1), diagonalDistance);
                if (hit.collider != null && !hit.collider.isTrigger && hit.collider.tag != "Player")
                {
                    valid = false;
                }
                nodeAtBottomRight = new NodeConnection(this, grid.Nodes[x + 1, y + 1], valid);
            }

        }

        if (y - 1 > 0)
        {
            valid = true;
            hit = Physics2D.Raycast(position, new Vector2(0, 1), grid.heightBetweenPoints);
            if (hit.collider != null && !hit.collider.isTrigger && hit.collider.tag != "Player")
            {
                valid = false;
            }
            nodeAtTop = new NodeConnection(this, grid.Nodes[x, y - 2], valid);
        }


        if (y < grid.Height - 2)
        {
            valid = true;
            hit = Physics2D.Raycast(position, new Vector2(0, -1), grid.heightBetweenPoints);
            if (hit.collider != null && !hit.collider.isTrigger && hit.collider.tag != "Player")
            {
                valid = false;
            }
            nodeAtBottom = new NodeConnection(this, grid.Nodes[x, y + 2], valid);
        }
    }


}

