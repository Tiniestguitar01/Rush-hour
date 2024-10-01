using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeForGeneration : Node
{
    public NodeForGeneration(int[,] board, int depth, Vehicle vehicle) : base(board, depth)
    { 
        this.vehicle = vehicle;
        EvaluateCost(); 
    }

    public override List<Node> GetChildren()
    {
        List<Vehicle> vehicles = GetVehicles();

        List<Node> children = new List<Node>();

        for (int i = 0; i< vehicles.Count;i++)
        {
            int[,] board = (int[,])this.board.Clone();
            for (int j = 0; j < vehicles[i].possibleMoves.Count; j++)
            {
                Node node = CreateChild(vehicles[i], vehicles[i].possibleMoves[j], board);
                children.Add(node);
            }
        }

        return children;
    }

    public override Node CreateChild(Vehicle vehicle, int[] position, int[,] board)
    {
        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, position, board, false);

        Node newNode = new NodeForGeneration(board, depth + 1,vehicle);

        newNode.parent = this;
        return newNode;
    }

    public override void EvaluateCost()
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

        cost -= (vehicle.maxDistanceForward + vehicle.maxDistanceBackward)/2;
    }
}
