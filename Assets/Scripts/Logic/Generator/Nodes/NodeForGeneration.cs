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
        List<Place> places = InstanceCreator.GetBoard().places;
        int currentId = InstanceCreator.GetPuzzleGenerator().id;

        List<Node> children = new List<Node>();

        foreach (Place place in places)
        {
            int[,] board = (int[,])this.board.Clone();

            Vehicle newVehicle = new Vehicle(currentId, place.size,place.placePosition, place.direction,board);

            CreateChild(newVehicle,board);
        }

        return children;
    }

    public Node CreateChild(Vehicle vehicle, int[,] board)
    {
        Node newNode = new NodeForGeneration(board, depth + 1, vehicle);
        newNode.parent = this;
        return newNode;
    }

    public override void EvaluateCost()
    {
        //distance from end
        float x = Mathf.Pow(0 - vehicle.startPosition[0], 2);
        float y = Mathf.Pow(2 - vehicle.startPosition[1], 2);
        this.cost = (int)Mathf.Sqrt(x + y);

        //this.cost += 0 - vehicle.startPosition[0];
        //this.cost += 2 - vehicle.startPosition[1];

        //direction variation
        if (parent.vehicle.direction == vehicle.direction)
        {
            this.cost += 3;
        }

        cost += vehicle.maxDistanceForward + vehicle.maxDistanceBackward;
        cost += depth;
    }
}
