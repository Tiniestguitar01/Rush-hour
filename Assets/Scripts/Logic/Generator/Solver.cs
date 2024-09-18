using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

    public class Solver : MonoBehaviour
    {
        public static Solver Instance;

        int steps = 0;

        public Node resultNode;

        void Awake()
        {
            Instance = this;
        }

        public bool BestFirstSearch(int[,] firstBoard)
        {
            Graph graph = new Graph();
            Node firstNode = new Node(firstBoard);
            graph.AddToOpenList(firstNode);
            steps = 0;
            while (graph.openList.Count != 0 && steps < 3000)
            {
                Node bestNode;
                bestNode = graph.openList.First();

                graph.RemoveFromOpenList(bestNode);

                List<Node> children = bestNode.GetChildren();

                foreach (Node child in children)
                {
                if (child.cost == 0)
                    {
                        resultNode = child;
                        return true;
                    }
                    else
                    {

                        if (!graph.openList.Any(node => node.Equals(child)) && !graph.closedList.Any(node => node.Equals(child)))
                        {
                            graph.AddToOpenList(child);
                            child.parent = bestNode;
                        }
                    }
                }
                steps++;
            }
            return false;

        }
    }
