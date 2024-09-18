using NUnit.Framework;

public class GraphTests
{

    [Test]
    public void ShouldAddToOpenList()
    {
        Graph graph = new Graph();

        Board.Instance = new Board();
        Board.Instance.size = 6;
        Board.Instance.GenerateBoard();

        Node node = new Node(Board.Instance.board);

        graph.AddToOpenList(node);

        Assert.Contains(node,graph.openList);
    }

    [Test]
    public void ShouldRemoveFromOpenList()
    {
        Graph graph = new Graph();

        Board.Instance = new Board();
        Board.Instance.size = 6;
        Board.Instance.GenerateBoard();

        Node node = new Node(Board.Instance.board);

        graph.AddToOpenList(node);

        graph.RemoveFromOpenList(node);

        //Assert.Contains(node, graph.closedList);
    }
}
