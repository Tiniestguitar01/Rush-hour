using System.Collections.Generic;

public class Graph
{
    public List<Node> openList;
    public List<Node> closedList;

    public Graph()
    {
        openList = new List<Node>();
        closedList = new List<Node>();
    }
}
