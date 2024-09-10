using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CostMiniMax
{
    Minimise = 0,
    Maximise = 1,
}

public class Solver : MonoBehaviour
{
    public static Solver Instance;

    int steps = 0;

    void Start()
    {
        Instance = this;
    }

    public bool SearchSolution(CostMiniMax miniMax, int[,] firstBoard)
    {
        Graph graph = new Graph();
        Node firstNode = new Node(firstBoard);
        graph.AddToOpenList(firstNode);
        steps = 0;

        // && steps < (int)GameData.Instance.difficulty
        while (graph.openList.Count != 0)
        {
            Node bestNode;
            if (miniMax == CostMiniMax.Minimise)
            {
                bestNode = graph.openList.First();
            }
            else
            {
                bestNode = graph.openList.Last();
            }

            graph.RemoveFromOpenList(bestNode);

            List<Node> children = bestNode.GetChildren(graph);
            foreach (Node child in children)
            {
                //cost-ot lehetne a difficulty-hoz kötni
                if (child.cost == 0)
                {
                    return true;
                }
                else
                {
                    if (!graph.Contains(child))
                    {
                        graph.AddToOpenList(child);
                    }
                }
            }
            steps++;
        }
        return false;

    }
}
