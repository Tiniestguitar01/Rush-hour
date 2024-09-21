using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>, IEquatable<Node>
{
    public int[,] board;
    public int depth;
    public float cost;

    public Vehicle movedVehicle;
    
    public Node parent;

    public int[,] goalBoard;

    public Node(int[,] board, int depth)
    {
        this.board = board;
        this.depth = depth;
        EvaluateCostForSolution();
    }

    public Node(int[,] board, int depth, int[,] goalBoard)
    {
        this.board = board;
        this.depth = depth;
        this.goalBoard = goalBoard;
        EvaluateCostCompareBoard(goalBoard);
    }

    public int CompareTo(Node other)
    {
        return this.cost.CompareTo(other.cost);
    }

    public bool Equals(Node other)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != other.board[i, j])
                    return false;
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(board, cost);
    }

    public List<Node> GetChildren()
    {
        List<Vehicle> vehicles = GetVehicles();

        List<Node> children = new List<Node>();

        foreach (Vehicle vehicle in vehicles)
        {
            int[,] board = (int[,])this.board.Clone();

            for (int i = 0; i < vehicle.possibleMoves.Count; i++)
            {
                Node node = CreateChild(vehicle, vehicle.possibleMoves[i], board);
                children.Add(node);
            }
        }

        return children;
    }

    public List<Vehicle> GetVehicles()
    {
        //Get all vehicles
        List<Vehicle> vehicles = new List<Vehicle>();

        int boardSize = InstanceCreator.GetBoard().size;
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

    public Node CreateChild(Vehicle vehicle, int[] position, int[,] board)
    {
        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, position, board);

        Node newNode;
        if (goalBoard != null)
        {
            newNode = new Node(board, depth + 1, goalBoard);
        }
        else
        { 
            newNode = new Node(board, depth + 1); 
        }

        newNode.parent = this;
        newNode.movedVehicle = vehicle;
        return newNode;
    }

    public void EvaluateCostForSolution()
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
        cost += depth;
    }

    public void EvaluateCostCompareBoard(int[,] goalBoard)
    {
        cost += (board.GetLength(0) * board.GetLength(1));
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(0); j++)
            {
                if (board[i,j] == goalBoard[i,j])
                {
                    cost--;
                }
            }   
        }
    }
}
