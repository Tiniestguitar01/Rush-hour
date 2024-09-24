using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    public List<Vehicle> vehicles = new List<Vehicle>();

    const int numberOfCars = 12;
    const int numberOfTrucks = 4;

    public int truckCount = 0;
    public int carCount = 0;

    public Board boardInstance;
    ModifyBoard modifyBoardInstance;
    Solver solverInstance;
    SpawnVehicles spawnVehicleInstance;

    public int[,] resultBoard;

    async void Start()
    {
        boardInstance = InstanceCreator.GetBoard();
        modifyBoardInstance = InstanceCreator.GetModifyBoard();
        solverInstance = InstanceCreator.GetSolver();
        spawnVehicleInstance = InstanceCreator.GetSpawnVehicles();
        await GeneratePuzzle();
    }

    public async Task<bool> GeneratePuzzle()
    {
        DeleteVehicles();

        boardInstance.GenerateBoard();

        InstanceCreator.GetSpawnGrid().Spawn();
        

        //el kellene indítani az algoritmust
        Graph graph = new Graph();
        Vehicle firstVehicle = CreateVehicle(1, 2, new int[] { Random.Range(1, boardInstance.size - 1), 2 }, Direction.Vertical);
        await InsertVehicle(CreateVehicle(1, firstVehicle.size, firstVehicle.startPosition, firstVehicle.direction), boardInstance.board);
        Node firstNode = new NodeForGeneration(boardInstance.board, 0, firstVehicle);
        graph.openList.Add(firstNode);

        while (graph.openList.Count != 0)
        {
            graph.openList.Sort();
            graph.openList.Reverse();

            Node bestNode = graph.openList.First();
            Debug.Log(bestNode.vehicle.ToString());
            await InsertVehicle(CreateVehicle(vehicles.Count, bestNode.vehicle.size, bestNode.vehicle.startPosition, bestNode.vehicle.direction), boardInstance.board);

            graph.openList.RemoveAt(0);
            graph.closedList.Add(bestNode);
            List<Node> children = bestNode.GetChildren();
            for (int nodeIndex = 0; nodeIndex < children.Count; nodeIndex++)
            {
                if ((children[nodeIndex].cost - children[nodeIndex].depth) >= 20 )
                {
                    resultBoard = children[nodeIndex].board;
                    PrintBoard(resultBoard);
                    Debug.Log("Yeeeee");
                    spawnVehicleInstance.Spawn();
                    return await Task.FromResult(true);
                }
                else
                {
                    if (!graph.openList.Any((node) => node.Equals(children[nodeIndex])) && !graph.closedList.Any((node) => node.Equals(children[nodeIndex])))
                    {
                        PrintBoard(children[nodeIndex].board);
                        graph.openList.Add(children[nodeIndex]);
                    }
                }
            }
            await Task.Yield();
        }
        return await Task.FromResult(false);
    }

    public Vehicle CreateVehicle(int id, int size, int[] startPosition, Direction direction)
    {
        Vehicle vehicle = new Vehicle(id, size, startPosition, direction,boardInstance.board);
        return vehicle;
    }

    public async Task<bool> InsertVehicle(Vehicle vehicle, int[,] board)
    {
        //ezt �t kell �rni 
        List<int[]> position = vehicle.GetPosition();
        modifyBoardInstance.InsertVehicle(vehicle, board);

        bool solvable = await solverInstance.Search(boardInstance.board,true);
        if (!solvable)
        {
            boardInstance.places.RemoveAll(place => place.placePosition[0] == position[0][0] && place.placePosition[1] == position[0][1]);

            modifyBoardInstance.RemoveVehicle(vehicle, board);

            return await Task.FromResult(false);
        }
        vehicles.Add(vehicle);
        for (int x = 0; x < vehicle.size; x++)
        {
            boardInstance.places.RemoveAll(place => place.placePosition[0] == position[x][0] && place.placePosition[1] == position[x][1]);
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
            boardInstance.places.RemoveAll(place => place.size == 2);
        }
        if (truckCount == numberOfTrucks)
        {
            boardInstance.places.RemoveAll(place => place.size == 3);
        }

        return await Task.FromResult(true);
    }

    public void RemovePlaces(int[] position)
    {
        for (int direction = 0; direction < 2; direction++)
        {
            if (position[direction] - 2 >= 0)
            {
                boardInstance.places.RemoveAll(place => place.placePosition[direction] == position[direction] - 2 && place.placePosition[1 - direction] == position[1 - direction] && place.size == 3 && place.direction == (Direction)direction);
            }

            if (position[direction] - 1 >= 0)
            {
                boardInstance.places.RemoveAll(place => place.placePosition[direction] == position[direction] - 1 && place.placePosition[1 - direction] == position[1 - direction] && place.size == 3 && place.direction == (Direction)direction);
                boardInstance.places.RemoveAll(place => place.placePosition[direction] == position[direction] - 1 && place.placePosition[1 - direction] == position[1 - direction] && place.size == 2 && place.direction == (Direction)direction);
            }
        }
    }

    public void DeleteVehicles()
    {
        foreach (GameObject vehicle in spawnVehicleInstance.vehicleGOs)
        {
            Destroy(vehicle);
        }

        spawnVehicleInstance.vehicleGOs.Clear();
        vehicles.Clear();

        truckCount = 0;
        carCount = 0;
    }

    public void PrintBoard(int[,] printBoard)
    {
        string boardString = "z:  0   1   2   3   4   5\n";
        for (int x = 0; x < boardInstance.size; x++)
        {
            boardString += x + ":";

            for (int z = 0; z < boardInstance.size; z++)
            {
                boardString += "  " + printBoard[x, z];
            }

            boardString += "\n";
        }

        Debug.Log(boardString);
    }
}
