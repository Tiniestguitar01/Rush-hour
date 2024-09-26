using PlasticGui.WorkspaceWindow.BranchExplorer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Codice.CM.WorkspaceServer.DataStore.WkTree.WriteWorkspaceTree;

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
        
        await InsertVehicle(CreateVehicle(1, 2, new int[] { Random.Range(1,boardInstance.size - 1), 2 }, Direction.Vertical), boardInstance.board);

        await GenerateVehicles();

        Graph graph = new Graph();
        Node firstNode = new NodeForGeneration(boardInstance.board, 0, vehicles[0]);
        graph.openList.Add(firstNode);
        int steps = 0;
        while (graph.openList.Count != 0 && steps < 1000)
        {
            graph.openList.Sort();

            Node bestNode = graph.openList.Last();
            graph.openList.RemoveAt(graph.openList.Count - 1);
            graph.closedList.Add(bestNode);

            boardInstance.board = (int[,])bestNode.board.Clone();
            resultBoard = (int[,])boardInstance.board.Clone();

            vehicles = bestNode.GetVehicles();

            List<Node> children = bestNode.GetChildren();

            for (int nodeIndex = 0; nodeIndex < children.Count; nodeIndex++)
            {
                if (children[nodeIndex].cost >= 5)
                {
                    boardInstance.board = (int[,])children[nodeIndex].board.Clone();
                    resultBoard = (int[,])boardInstance.board.Clone();
                    PrintBoard(resultBoard);
                    Debug.Log("Siker yeee");
                    spawnVehicleInstance.Spawn();
                    return await Task.FromResult(true);
                }
                else
                {
                    if (!graph.openList.Any((node) => node.Equals(children[nodeIndex])) && !graph.closedList.Any((node) => node.Equals(children[nodeIndex])))
                    {
                        graph.openList.Add(children[nodeIndex]);
                    }
                }
            }
            steps++;
            Debug.Log("generation: " + steps);
            await Task.Yield();
        }

        vehicles.Sort();
        spawnVehicleInstance.Spawn();

        return await Task.FromResult(false);
    }
    public async Task GenerateVehicles()
    {
        for (int id = 2; id < 6 && boardInstance.places.Count > 0; id++)
        {
            int random = Random.Range(0, boardInstance.places.Count);
            await InsertVehicle(CreateVehicle(vehicles.Count, boardInstance.places[random].size, boardInstance.places[random].placePosition, boardInstance.places[random].direction), boardInstance.board);
        }

        for (int i = 0; i < vehicles.Count; i++)
        {
            for (int j = 1; j < vehicles[i].possibleMoves.Count; j++)
            {
                Place place = boardInstance.GetRandomPlaceByCoordinate(vehicles[i].possibleMoves[j]);
                if (place != null)
                {
                    await InsertVehicle(CreateVehicle(vehicles.Count, place.size, place.placePosition, place.direction), boardInstance.board);
                }
            }
        }
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
        PrintBoard(boardInstance.board);

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
