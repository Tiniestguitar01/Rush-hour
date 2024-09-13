using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>, ICloneable
{
    public int[,] board;
    public int cost;

    public Node(int[,] board)
    {
        this.board = board;
    }

    public int CompareTo(Node other)
    {
        return this.cost.CompareTo(other.cost);
    }

    public object Clone()
    {
        return new Node((int[,])board.Clone());
    }

    public List<Node> GetChildren(Graph graph)
    {
        List<Vehicle> vehicles = GetVehicles();

        List<Node> children = new List<Node>();

        foreach (Vehicle vehicle in vehicles)
        {
            int[,] board = (int[,])this.board.Clone();

            for (int i = 1; i < vehicle.possibleMoves.Count; i++)
            {
                children.Add(CreateChildren(graph, vehicle, vehicle.possibleMoves[i], board));
            }
        }

        return children;
    }

    List<Vehicle> GetVehicles()
    {
        //Get all vehicles
        List<Vehicle> vehicles = new List<Vehicle>();

        int boardSize = Board.Instance.size;
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                if (board[x, z] != 0)
                {
                    int size = 2;
                    Direction direction = new Direction();

                    if (z + 1 < boardSize && board[x, z + 1] == board[x, z])
                    {
                        direction = Direction.Horizontal;

                        if (z + 2 < boardSize && board[x, z + 2] == board[x, z])
                        {
                            size = 3;
                        }
                    }
                    else
                    {
                        direction = Direction.Vertical;

                        if (x + 2 < boardSize && board[x + 2, z] == board[x, z])
                        {
                            size = 3;
                        }
                    }

                    if (!vehicles.Exists(v => board[x, z] == v.id))
                    {
                        Vehicle vehicle = new Vehicle(board[x, z], size, new int[] { x, z }, direction, board);
                        vehicles.Add(vehicle);
                    }
                }
            }
        }

        return vehicles;
    }

    Node CreateChildren(Graph graph,Vehicle vehicle, int[] position, int[,] board)
    {
        ModifyBoard.Instance.MoveVehicle(vehicle, position, board);

        Node newNode = new Node(board);
        newNode.EvaluateCost();

        return newNode;
    }

    void EvaluateCost()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            if (board[i, 2] != 1 && board[i, 2] != 0)
            {
                cost += board.GetLength(0);
            }
            
            if (board[i, 2] == 1)
            {
                cost += i;
                break;
            }
        }
    }
}
