using System.Collections.Generic;
using UnityEngine;

    public class Graph
    {
        public List<Node> openList;
        public List<Node> closedList;

        public Graph()
        {
            openList = new List<Node>();
            closedList = new List<Node>();
        }

        public void AddToOpenList(Node node)
        {
            openList.Add(node);
        }

        public void RemoveFromOpenList(Node node)
        {
            for(int i = 0; i < openList.Count;i++)
            {
                if(openList[i].Equals(node))
                {
                    openList.RemoveAt(i);
                }
            }
            closedList.Add(node);
        }

    }
