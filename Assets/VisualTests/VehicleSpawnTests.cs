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

    private GameObject car = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/BrokenVector/LowPolyCarPack/Prefabs/Car_3_Red.prefab");

    [SetUp]
    public void Init()
    {
        GameObject InstanceCreatorGO = new GameObject("InstanceCreator");
        GameObject GenerationManagerGO = new GameObject("GenerationManager");
        GameObject VisualManagerGO = new GameObject("VisualManager");

        InstanceCreatorGO.AddComponent<InstanceCreator>();
        GenerationManagerGO.AddComponent<Board>();
        GenerationManagerGO.AddComponent<PuzzleGenerator>();
        VisualManagerGO.AddComponent<SpawnVehicles>();

        boardInstance = InstanceCreator.GetBoard();
        puzzleGeneratorInstance = InstanceCreator.GetPuzzleGenerator();
        puzzleGeneratorInstance.vehicles.Add(new Vehicle(1, 2, new int[] { 0, 2 }, Direction.Vertical, boardInstance.board));
        spawnVehiclesInstance = InstanceCreator.GetSpawnVehicles();

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

        Assert.AreEqual(new Vector3(vehicle.startPosition[0] * 3.5f, (0.5f * vehicle.size), vehicle.startPosition[1] * 3.5f), spawnVehiclesInstance.vehicleGOs[0].transform.position);

    }
}
