using System.Collections.Generic;
using UnityEngine;

    public class Graph
    {
        public SortedSet<Node> openList;
        public SortedSet<Node> closedList;

        public Graph()
        {
            openList = new SortedSet<Node>();
            closedList = new SortedSet<Node>();
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