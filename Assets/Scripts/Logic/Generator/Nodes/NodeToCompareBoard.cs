using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeToCompareBoard : Node
{
    public int[,] goalBoard;
    public NodeToCompareBoard(int[,] board, int depth, int[,] goalBoard) : base(board, depth)
    {
        this.goalBoard = goalBoard;
        EvaluateCost();
    }

    public override Node CreateChild(Vehicle vehicle, int[] position, int[,] board)
    {
        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, position, board, false);

        Node newNode = new NodeToCompareBoard(board, depth + 1, goalBoard);
        
        newNode.parent = this;
        newNode.vehicle = vehicle;
        return newNode;
    }

    public override void EvaluateCost()
    {
        cost += (board.GetLength(0) * board.GetLength(1));
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(0); j++)
            {
                Debug.Log(goalBoard[i, j]);
                if (board[i, j] == goalBoard[i, j])
                {
                    cost--;
                }
            }
        }
    }
}
