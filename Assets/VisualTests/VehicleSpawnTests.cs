using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;


public class VehicleSpawnTests
{
    List<Vehicle> vehicles;
    SpawnVehicles spawnVehiclesInstance;
    Board boardInstance;
    PuzzleGenerator puzzleGeneratorInstance;
    SpawnGrid spawnGridInstance;

    private GameObject car = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/BrokenVector/LowPolyCarPack/Prefabs/Car_3_Red.prefab");

    [SetUp]
    public void Init()
    {
        GameObject InstanceCreatorGO = new GameObject("InstanceCreator");
        GameObject GenerationManagerGO = new GameObject("GenerationManager");
        GameObject VisualManagerGO = new GameObject("VisualManager");
        GameObject MiscLogicManagerGO = new GameObject("MiscLogicManager");
        GameObject UIManagerGO = new GameObject("UIManager");

        InstanceCreatorGO.AddComponent<InstanceCreator>();
        UIManagerGO.AddComponent<UIManager>();
        MiscLogicManagerGO.AddComponent<GameData>();
        boardInstance = GenerationManagerGO.AddComponent<Board>();
        boardInstance.Start();
        puzzleGeneratorInstance = GenerationManagerGO.AddComponent<PuzzleGenerator>();
        puzzleGeneratorInstance.Start();
        spawnVehiclesInstance = VisualManagerGO.AddComponent<SpawnVehicles>();
        spawnGridInstance = VisualManagerGO.AddComponent<SpawnGrid>();
        spawnVehiclesInstance.Start();
        spawnGridInstance.distance = 1;
        spawnGridInstance.offset = 0;

        puzzleGeneratorInstance.vehicles.Add(new Vehicle(1, 2, new int[] { 0, 2 }, Direction.Vertical, boardInstance.board));

        spawnVehiclesInstance.cars.Add(car);
    }

    [Test]
    public void VehicleSpawnTestsSimplePasses()
    {
        spawnVehiclesInstance.Spawn();
        VehicleComponent vehicleComponent = spawnVehiclesInstance.vehicleGOs[0].transform.GetChild(0).GetComponent<VehicleComponent>();
        Vehicle vehicle = puzzleGeneratorInstance.vehicles[0];

        Assert.AreEqual(vehicle.id, vehicleComponent.id);
        Assert.AreEqual(vehicle.size, vehicleComponent.size);
        Assert.AreEqual(vehicle.startPosition, vehicleComponent.startPosition);
        Assert.AreEqual(vehicle.direction, vehicleComponent.direction);

        Assert.AreEqual(new Vector3(vehicle.startPosition[0] * spawnGridInstance.distance + (boardInstance.maxBoardSize - boardInstance.size) * spawnGridInstance.offset, 0.99f, vehicle.startPosition[1] * spawnGridInstance.distance + (boardInstance.maxBoardSize - boardInstance.size) * spawnGridInstance.offset), spawnVehiclesInstance.vehicleGOs[0].transform.position);

    }
}
