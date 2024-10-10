using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeForSolution : Node
{
    public NodeForSolution(int[,] board, int depth) : base(board, depth) { EvaluateCost(); }

    public override Node CreateChild(Vehicle vehicle, int[] position, int[,] board)
    {
        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, position, board, true);

        Node newNode = new NodeForSolution(board, depth + 1);

        newNode.parent = this;
        newNode.vehicle = vehicle;
        return newNode;
    }

    public override void EvaluateCost()
    {
        int boardSize = board.GetLength(0);
        for (int i = 0; i < boardSize; i++)
        {
            if (board[i, 2] != 1 && board[i, 2] != 0)
            {
                cost += boardSize;
            }

            if (board[i, 2] == 1)
            {
                cost += i;
                break;
            }
        }

        //int maxGreedyCost = ((boardSize - 2) * boardSize) + (boardSize - 2);
        //float dijkstraWeight = cost/maxGreedyCost;

        cost += depth;
    }
}
