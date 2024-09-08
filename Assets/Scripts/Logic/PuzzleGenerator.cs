using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    public static PuzzleGenerator Instance;
    public List<Vehicle> vehicles = new List<Vehicle>();

    const int numberOfCars = 12;
    const int numberOfTrucks = 4;

    int truckCount = 0;
    int carCount = 0;

    int steps = 0;

    void Start()
    {
        Instance = this;
        InsertVehicle(CreateVehicle(1, 2, new int[] { Random.Range(2,Board.Instance.size - 1), 2 }, Direction.Vertical),Board.Instance.board);

        GenerateVehicles();
        PrintBoard(Board.Instance.board);

        SpawnGrid.Instance.Spawn();
        SpawnVehicles.Instance.Spawn();
    }

    void GenerateVehicles()
    {
        for (int id = 2; id < 16 && Board.Instance.places.Count > 0; id++)
        {
            int random = Random.Range(0, Board.Instance.places.Count);

            if(!InsertVehicle(CreateVehicle(id, Board.Instance.places[random].size, Board.Instance.places[random].placePosition, Board.Instance.places[random].direction), Board.Instance.board))
            {
                id--;
            }
        }
    }

    Vehicle CreateVehicle(int id, int size, int[] startPosition, Direction direction)
    {
        Vehicle vehicle = new Vehicle(id, size, startPosition, direction);
        return vehicle;
    }

    public bool InsertVehicle(Vehicle vehicle, int[,] board)
    {
        //ezt át kell írni 
        List<int[]> position = vehicle.GetPosition();
        ModifyBoard.Instance.InsertVehicle(vehicle, board);

        if (!SearchSolution())
        {
            for (int x = 0; x < vehicle.size; x++)
            {
                Board.Instance.places.RemoveAll(place => place.placePosition[0] == position[x][0] && place.placePosition[1] == position[x][1]);
            }

            Debug.Log(vehicle.ToString());
            ModifyBoard.Instance.RemoveVehicle(vehicle, board);

            return false;
        }

        vehicles.Add(vehicle);
        for (int x = 0; x < vehicle.size; x++)
        {
            Board.Instance.places.RemoveAll(place => place.placePosition[0] == position[x][0] && place.placePosition[1] == position[x][1] );
            RemovePlaces(new int[] { position[x][0], position[x][1] });
        }

        if (vehicle.size == 2)
        {
            carCount++;
        }
        else
        {
            truckCount++;
        }

        if (carCount == numberOfCars)
        {
            Board.Instance.places.RemoveAll(place => place.size == 2);
        }
        if (truckCount == numberOfTrucks)
        {
            Board.Instance.places.RemoveAll(place => place.size == 3);
        }

        return true;
    }

    public void RemovePlaces(int[] position)
    {
        for (int direction = 0; direction < 2; direction++)
        {
            if (position[direction] - 2 >= 0)
            {
                Board.Instance.places.RemoveAll(place => place.placePosition[direction] == position[direction] - 2 && place.placePosition[1 - direction] == position[1 - direction] && place.size == 3 && place.direction == (Direction)direction);
            }

            if (position[direction] - 1 >= 0)
            {
                Board.Instance.places.RemoveAll(place => place.placePosition[direction] == position[direction] - 1 && place.placePosition[1 - direction] == position[1 - direction] && place.size == 3 && place.direction == (Direction)direction);
                Board.Instance.places.RemoveAll(place => place.placePosition[direction] == position[direction] - 1 && place.placePosition[1 - direction] == position[1 - direction] && place.size == 2 && place.direction == (Direction)direction);
            }
        }
    }

    public bool SearchSolution()
    {
        Graph graph = new Graph();
        Node firstNode = new Node(Board.Instance.board);
        graph.AddToOpenList(firstNode);
        steps = 0;

        while (graph.openList.Count != 0 && steps < 100)
        {

            Node bestNode = graph.openList.First();

            Debug.Log("First Node");
            PrintBoard(bestNode.board);

            Debug.Log("graph.openList");

            foreach (Node node in graph.openList)
            {
                PrintBoard(node.board);
            }

            graph.RemoveFromOpenList(bestNode);

            List<Node> children = bestNode.GetChildren(graph);
            foreach (Node child in children)
            {
                if (child.cost == 0)
                {
                    Debug.Log("yaay :)");
                    return true;
                }
                else
                {
                    if (!graph.openList.Contains(child) && !graph.closedList.Contains(child))
                    {
                        graph.AddToOpenList(child);
                    }
                }
            }
            steps++;
        }

        Debug.Log("big sad :(");
        return false;

    }

    public void PrintBoard(int[,] board)
    {
        string boardString = "z:  0   1   2   3   4   5\n";
        for (int x = 0; x < Board.Instance.size; x++)
        {
            boardString += x + ":";

            for (int z = 0; z < Board.Instance.size; z++)
            {
                boardString += "  " + board[x, z];
            }

            boardString += "\n";
        }

        Debug.Log(boardString);
    }
}
