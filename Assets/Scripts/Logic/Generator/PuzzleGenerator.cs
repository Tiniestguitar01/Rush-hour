using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    public List<Vehicle> vehicles = new List<Vehicle>();

    const int numberOfCars = 12;
    const int numberOfTrucks = 4;

    int truckCount = 0;
    int carCount = 0;

    Board boardInstance;
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

        await InsertVehicle(CreateVehicle(1, 2, new int[] { 0, 2 }, Direction.Vertical), boardInstance.board);

        foreach (Place place in boardInstance.places)
        {
            place.CalculateCost(vehicles[0].startPosition, Direction.Vertical);
        }
        GenerateVehicles();

        resultBoard = (int[,])boardInstance.board.Clone();
        PrintBoard(resultBoard);

        return await Task.FromResult(true);
    }

    async void GenerateVehicles()
    {
        for (int id = 2; id < 10 && boardInstance.places.Count > 0; id++)
        {
            foreach (Place place in boardInstance.places)
            {
                place.CalculateCost(vehicles[0].startPosition, vehicles[vehicles.Count - 1].direction);
            }

            boardInstance.places.Sort();

            int random = Random.Range(0, Mathf.Min( 10, boardInstance.places.Count));
            bool result = await InsertVehicle(CreateVehicle(id, boardInstance.places[random].size, boardInstance.places[random].placePosition, boardInstance.places[random].direction), boardInstance.board);
            if (!result)
            {
                id--;
            }
        }

        spawnVehicleInstance.Spawn();
    }

    Vehicle CreateVehicle(int id, int size, int[] startPosition, Direction direction)
    {
        Vehicle vehicle = new Vehicle(id, size, startPosition, direction, boardInstance.board);
        return vehicle;
    }

    public async Task<bool> InsertVehicle(Vehicle vehicle, int[,] board)
    {
        //ezt �t kell �rni 
        List<int[]> position = vehicle.GetPosition();
        modifyBoardInstance.InsertVehicle(vehicle, board);

        //bool solvable = await solverInstance.BestFirstSearch(boardInstance.board);
        /*if (!solvable)
        {
            boardInstance.places.RemoveAll(place => place.placePosition[0] == position[0][0] && place.placePosition[1] == position[0][1]);

            modifyBoardInstance.RemoveVehicle(vehicle, board);

            return await Task.FromResult(false);
        }*/

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

    void DeleteVehicles()
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
