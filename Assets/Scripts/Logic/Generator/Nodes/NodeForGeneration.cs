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
        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, position, board);

        Node newNode = new NodeForGeneration(board, depth + 1,vehicle);

        newNode.parent = this;
        return newNode;
    }

    public override void EvaluateCost()
    {
        if (vehicle.id != 1)
        {
            float x = Mathf.Pow(6 - vehicle.startPosition[0], 2);
            float y = Mathf.Pow(2 - vehicle.startPosition[1], 2);
            this.cost = (int)Mathf.Sqrt(x + y);
        }
        else
        {
            float x = Mathf.Pow(0 - vehicle.startPosition[0], 2);
            float y = Mathf.Pow(2 - vehicle.startPosition[1], 2);
            this.cost = (int)Mathf.Sqrt(x + y);
        }

        //this.cost += 0 - vehicle.startPosition[0];
        //this.cost += 2 - vehicle.startPosition[1];

        if (parent != null)
        {
            if (parent.vehicle.direction == vehicle.direction)
            {
                this.cost -= 3;
            }
        }

        //cost -= vehicle.maxDistanceForward - vehicle.maxDistanceBackward;
    }
}
