using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    public List<Vehicle> vehicles;

    int numberOfCars = 12;
    int numberOfTrucks = 4;

    public int truckCount = 0;
    public int carCount = 0;

    public Board boardInstance;
    ModifyBoard modifyBoardInstance;
    Solver solverInstance;
    SpawnVehicles spawnVehicleInstance;
    SpawnGrid spawnGridInstance;
    UIManager uiManagerInstance;
    GameData gameDataInstance;

    public int[,] resultBoard;

    public async void Start()
    {
        vehicles = new List<Vehicle>();
        boardInstance = InstanceCreator.GetBoard();
        modifyBoardInstance = InstanceCreator.GetModifyBoard();
        solverInstance = InstanceCreator.GetSolver();
        spawnVehicleInstance = InstanceCreator.GetSpawnVehicles();
        uiManagerInstance = InstanceCreator.GetUIManager();
        gameDataInstance = InstanceCreator.GetGameData();
        spawnGridInstance = InstanceCreator.GetSpawnGrid();

        numberOfTrucks = boardInstance.board.GetLength(0) - 2;
        numberOfCars = numberOfCars * 4;
    }

    public async Task<bool> GeneratePuzzle()
    {

        DeleteVehicles();

        boardInstance.GenerateBoard();
        spawnGridInstance.Spawn();

        await InsertVehicle(CreateVehicle(1, 2, new int[] { Random.Range(Mathf.Min((int)(2 * (int)gameDataInstance.difficulty / 1.5f), boardInstance.size - 2), boardInstance.size - 1), 2 }, Direction.Vertical), boardInstance.board);

        for (int i = 0; i < (int)gameDataInstance.difficulty * 2; i++)
        {
            //boardInstance.GetFreeSpaces();
            await GenerateVehicles();
        }

        int iteration = 0;
        while (boardInstance.places.Count > 0 && ((int)gameDataInstance.difficulty * boardInstance.size) == iteration)
        {
            int random = Random.Range(0, boardInstance.places.Count);
            await InsertVehicle(CreateVehicle(vehicles.Count + 1, boardInstance.places[random].size, boardInstance.places[random].placePosition, boardInstance.places[random].direction), boardInstance.board);
            iteration++;
        }

        resultBoard = (int[,])boardInstance.board.Clone();

        vehicles.Sort();
        spawnVehicleInstance.Spawn();

        return await Task.FromResult(true);
    }
    public async Task GenerateVehicles()
    {
        int id = vehicles.Count - 1;
        for (int i = 0; i < boardInstance.size * (int)gameDataInstance.difficulty; i++)
        {

            bool firstResult = false;
            bool secondResult = false;

            List<Place> forwardPlaceList = boardInstance.GetPlace(vehicles[id], true);
            if (forwardPlaceList != null)
            {
                while (forwardPlaceList.Count > 0 && !firstResult)
                {
                    Place place = forwardPlaceList[Random.Range(0, forwardPlaceList.Count - 1)];
                    forwardPlaceList.Remove(place);
                    firstResult = await InsertVehicle(CreateVehicle(vehicles.Count + 1, place.size, place.placePosition, place.direction), boardInstance.board);
                }
            }

            List<Place> backwardPlaceList = boardInstance.GetPlace(vehicles[id], false);
            if (backwardPlaceList != null)
            {
                while (backwardPlaceList.Count > 0 && !secondResult)
                {
                    Place place = backwardPlaceList[Random.Range(0, backwardPlaceList.Count - 1)];
                    backwardPlaceList.Remove(place);
                    secondResult = await InsertVehicle(CreateVehicle(vehicles.Count + 1, place.size, place.placePosition, place.direction), boardInstance.board);
                }
            }

            if (firstResult || (!firstResult && secondResult))
            {
                id++;
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
        List<int[]> position = vehicle.GetPosition();
        modifyBoardInstance.InsertVehicle(vehicle, board);

        bool solvable = await solverInstance.Search(boardInstance.board, true);
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

        foreach (GameObject cell in spawnGridInstance.instantiatedCells)
        {
            Destroy(cell);
        }

        spawnVehicleInstance.vehicleGOs.Clear();
        spawnGridInstance.instantiatedCells.Clear();

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
