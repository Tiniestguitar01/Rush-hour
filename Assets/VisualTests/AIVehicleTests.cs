using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class AIVehicleTests
{
    GameObject redPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/red.prefab");
    GameObject car = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Car_2_Red.prefab");

    SpawnVehicles spawnVehiclesInstance;
    VehicleSpawner vehicleSpawnerInstance;
    StopVehicle stopVehicleInstance;

    [SetUp]
    public void Init()
    {
        GameObject VehicleSpawnerGO = new GameObject("Spawner");
        vehicleSpawnerInstance = VehicleSpawnerGO.AddComponent<VehicleSpawner>();

        GameObject InstanceCreatorGO = new GameObject("InstanceCreator");
        GameObject VisualManagerGO = new GameObject("VisualManager");
        GameObject stopVehicleGO = new GameObject("Stopper");
        stopVehicleInstance = stopVehicleGO.AddComponent<StopVehicle>();
        InstanceCreatorGO.AddComponent<InstanceCreator>();
        VisualManagerGO.AddComponent<SpawnVehicles>();
        spawnVehiclesInstance = InstanceCreator.GetSpawnVehicles();

        spawnVehiclesInstance.cars.Add(car);
    }

    [Test]
    public void AIVehicleSpawnTest()
    {
        vehicleSpawnerInstance.Spawn();
    }

    [Test]
    public void AIVehicleTest()
    {
        vehicleSpawnerInstance.Spawn();
    }

    [UnityTest]
    public IEnumerator VehicleStopperTest()
    {
        stopVehicleInstance.InstantiateRed(redPrefab);
        stopVehicleInstance.Stop();

        Assert.IsFalse(stopVehicleInstance.stopped);
        Assert.IsFalse(stopVehicleInstance.red.activeSelf);

        yield return  new WaitForSeconds(stopVehicleInstance.wait);

        Assert.IsTrue(stopVehicleInstance.stopped);
        Assert.IsTrue(stopVehicleInstance.red.activeSelf);
    }
}
