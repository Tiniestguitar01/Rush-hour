using System.Collections.Generic;
using System.Linq;
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
            graph.AddToOpenList((Node)firstNode.Clone());
            steps = 0;
            while (graph.openList.Count != 0)
            {
                Node bestNode;
                bestNode = graph.openList.First();

                graph.RemoveFromOpenList(bestNode);

                List<Node> children = bestNode.GetChildren();

                Debug.Log("----------------parent----------------");
                PuzzleGenerator.Instance.PrintBoard(bestNode.board);

                Debug.Log("----------------openlist----------------");
                foreach(Node n in graph.openList)
                {
                    PuzzleGenerator.Instance.PrintBoard(n.board);
                }

                Debug.Log("----------------closedlist----------------");

                Debug.Log("----------------children----------------");
                foreach(Node n in graph.closedList)
                {
                    PuzzleGenerator.Instance.PrintBoard(n.board);
                }

                foreach (Node child in children)
                {
                    PuzzleGenerator.Instance.PrintBoard(child.board);
                    Debug.Log(child.cost);

                    if (child.cost == 0)
                    {
                        resultNode = child;
                        return true;
                    }
                    else
                    {

                        Debug.Log("In openList: " + graph.openList.Contains((Node)child.Clone()));
                        Debug.Log("In closedList: " + graph.closedList.Contains((Node)child.Clone()));
                        if (!graph.openList.Contains((Node)child.Clone()) && !graph.closedList.Contains((Node)child.Clone()))
                        {
                            graph.AddToOpenList((Node)child.Clone());
                            child.parent = bestNode;
                        }
                    }
                }
                steps++;
            }
            return false;

        }
    }
