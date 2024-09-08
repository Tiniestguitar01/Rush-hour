using System.Collections.Generic;

public class Graph
{
    public SortedSet<Node> openList;
    public HashSet<Node> closedList;

    public Graph()
    {
        openList = new SortedSet<Node>();
        closedList = new HashSet<Node>();
    }

    public void AddToOpenList(Node node)
    {
        openList.Add(node);
    }

    public void RemoveFromOpenList(Node node)
    {
        openList.Remove(node);
        closedList.Add(node);
    }
}
