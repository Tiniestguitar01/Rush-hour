using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeForGeneration : Node
{
    public NodeForGeneration(int[,] board, int depth) : base(board, depth) { EvaluateCost(); }

    public override Node CreateChild(Vehicle vehicle, int[] position, int[,] board)
    {
        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, position, board);

        Node newNode = new NodeForSolution(board, depth + 1);

        newNode.parent = this;
        newNode.movedVehicle = vehicle;
        return newNode;
    }

    public override void EvaluateCost()
    {
        //distance from end
        //float x = Mathf.Pow(0 - x, 2);
        //float y = Mathf.Pow(2 - y, 2);
        //this.cost = (int)Mathf.Sqrt(x + y);

        //this.cost += targetPosition[0] - 0;
        //this.cost += targetPosition[1] - 2;

        //direction variation
        //if (prevDirection == direction)
        {
            this.cost += 3;
        }
        cost += depth;
    }
}
