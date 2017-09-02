namespace RaverSoft.YllisanSkies.Pathfinding
{
    public class NodeConnection
    {
        public Node parent;
        public Node node;
        public bool isValid;

        public NodeConnection(Node parent, Node node, bool isValid)
        {
            this.isValid = isValid;
            this.node = node;
            this.parent = parent;

            if (this.node != null && this.node.isBadNode)
            {
                this.isValid = false;
            }
            if (this.parent != null && this.parent.isBadNode)
            {
                this.isValid = false;
            }
        }
    }
}   