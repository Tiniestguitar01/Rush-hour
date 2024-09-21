using NUnit.Framework;

public class GraphTests
{
    Graph graph;
    Board board;
    Node node;

    [SetUp]
    public void Init()
    {
        graph = new Graph();

        board = InstanceCreator.GetBoard();

        board.size = 6;
        board.GenerateBoard();

        node = new Node(board.board, 0);
    }

    //[Test]
   /* public void ShouldAddToOpenList()
    {

        graph.AddToOpenList(node);

        Assert.Contains(node,graph.openList);
    }

    [Test]
    public void ShouldRemoveFromOpenList()
    {

        graph.AddToOpenList(node);

        graph.RemoveFromOpenList(node);

        //Assert.Contains(node, graph.closedList);
    }*/
}
