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
            while (graph.openList.Count != 0 && steps < 100)
            {
                Node bestNode;
                bestNode = graph.openList.First();

                graph.RemoveFromOpenList(bestNode);

                List<Node> children = bestNode.GetChildren();

                Debug.Log("----------------parent----------------");
                PuzzleGenerator.Instance.PrintBoard(bestNode.board);
                foreach (Node child in children)
                {
                    Debug.Log("----------------child----------------");
                    PuzzleGenerator.Instance.PrintBoard(child.board);
                    Debug.Log(child.cost);
                    /*Debug.Log("In openList: " + graph.openList.Contains((Node)child.Clone()));
                    Debug.Log("In closedList: " + graph.closedList.Contains((Node)child.Clone()));
                    Debug.Log("If: " + (!graph.openList.Contains(child) && !graph.closedList.Contains(child)));

                    Debug.Log("New openList: " + graph.openList.Any(node => node.Equals(child)));
                    Debug.Log("New closedList: " + graph.closedList.Any(node => node.Equals(child)));
                    Debug.Log("New If: " + (!graph.openList.Any(node => node.Equals(child)) && !graph.closedList.Any(node => node.Equals(child))));*/
                if (child.cost == 0)
                    {
                        resultNode = child;
                        return true;
                    }
                    else
                    {

                        if (!graph.openList.Contains(child.Clone()) && !graph.closedList.Contains(child.Clone()))
                        {
                            graph.AddToOpenList((Node)child.Clone());
                            child.parent = bestNode;
                        }

                        Debug.Log("----------------openlist----------------");
                        foreach (Node n in graph.openList)
                        {
                            PuzzleGenerator.Instance.PrintBoard(n.board);
                        Debug.Log(n.cost);
                        }
                        Debug.Log("----------------openlist----------------");
                        Debug.Log("----------------closedlist----------------");

                        foreach (Node n in graph.closedList)
                        {
                            PuzzleGenerator.Instance.PrintBoard(n.board);
                        Debug.Log(n.cost);
                    }
                        Debug.Log("----------------closedlist----------------");
                    }
                }
                steps++;
            }
            return false;

        }
    }
