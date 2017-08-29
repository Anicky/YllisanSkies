using UnityEngine;

public static class PathFinder
{     
	public static BreadCrumb FindPath(Grid world, Point start, Point end)
	{        
	    BreadCrumb bc = FindPathReversed(world, start, end);
        BreadCrumb[] temp = new BreadCrumb[256];

        if (bc != null)
        {
            int index = 0;
            while (bc != null)
            {
                temp[index] = bc;
                bc = bc.next;
                index++;
            }

            index -= 2;

            BreadCrumb current = new BreadCrumb(start);
            BreadCrumb head = current;

            while (index >= 0)
            {                
                current.next = new BreadCrumb(temp[index].position);
                current = current.next;
                index--;              
            }
            return head;
        }
        else
        {
            return null;
        }
	}
	
	private static BreadCrumb FindPathReversed(Grid world, Point start, Point end)
	{
	    MinHeap<BreadCrumb> openList = new MinHeap<BreadCrumb>(256);
	    BreadCrumb[,] brWorld = new BreadCrumb[world.Right, world.Top];
	    BreadCrumb node;
	    Point tmp;
	    int cost;
	    int diff;
	
	    BreadCrumb current = new BreadCrumb(start);
	    current.cost = 0;
	
	    BreadCrumb finish = new BreadCrumb(end);
	    brWorld[current.position.X, current.position.Y] = current;
	    openList.Add(current);
	
	    while (openList.Count > 0)
	    {
	        //Find best item and switch it to the 'closedList'
	        current = openList.ExtractFirst();
	        current.onClosedList = true;
	
	        //Find neighbours
	        for (int i = 0; i < surrounding.Length; i++)
	        {
                tmp = new Point(current.position.X + surrounding[i].X, current.position.Y + surrounding[i].Y);

                if (!world.ConnectionIsValid(current.position, tmp)) 
                    continue;                				                  

                //Check if we've already examined a neighbour, if not create a new node for it.
                if (brWorld[tmp.X, tmp.Y] == null)
                {
                    node = new BreadCrumb(tmp);
                    brWorld[tmp.X, tmp.Y] = node;
                }
                else
                {
                    node = brWorld[tmp.X, tmp.Y];
                }

                //If the node is not on the 'closedList' check it's new score, keep the best
                if (!node.onClosedList)
                {
                    diff = 0;
                    if (current.position.X != node.position.X)
                    {
                        diff += 1;
                    }
                    if (current.position.Y != node.position.Y)
                    {
                        diff += 1;
                    }

					int distance = (int)Mathf.Pow(Mathf.Max(Mathf.Abs (end.X - node.position.X), Mathf.Abs(end.Y - node.position.Y)), 2);
                    cost = current.cost + diff + distance;

                    if (cost < node.cost)
                    {
                        node.cost = cost;
                        node.next = current;
                    }

                    //If the node wasn't on the openList yet, add it 
                    if (!node.onOpenList)
                    {
                        //Check to see if we're done
                        if (node.Equals(finish))
                        {
                            node.next = current;
                            return node;
                        }
                        node.onOpenList = true;
                        openList.Add(node);
                    }
	            }
	        }
	    }
	    return null; //no path found
	}
	
	//Neighbour options
	//Our diamond pattern offsets top/bottom/left/right by 2 instead of 1
	private static Point[] surrounding = new Point[]{                         
		new Point(0, 2), new Point(-2, 0), new Point(2, 0), new Point(0,-2),	
        new Point(-1, 1), new Point(-1, -1), new Point(1, 1), new Point(1, -1)
	};
}

