using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeForSolution : Node
{
    public NodeForSolution(int[,] board, int depth) : base(board, depth) { EvaluateCost(); }

    public override Node CreateChild(Vehicle vehicle, int[] position, int[,] board)
    {
        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, position, board);

        Node newNode = new NodeForSolution(board, depth + 1);

        newNode.parent = this;
        newNode.vehicle = vehicle;
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
        cost += depth;
    }
}
