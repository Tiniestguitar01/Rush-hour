using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class AIVehicleTests
{
    GameObject vehiclePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Vehicle.prefab");
    GameObject train = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Low Poly Simple Urban City 3D Asset Pack/Prefabs/Vehicles/Trams/Train.prefab");

    SpawnVehicles spawnVehiclesInstance;
    VehicleSpawner vehicleSpawnerInstance;
    StopVehicle stopVehicleInstance;

    [SetUp]
    public void Init()
    {
        GameObject VehicleSpawnerGO = new GameObject("Spawner");
        vehicleSpawnerInstance = VehicleSpawnerGO.AddComponent<VehicleSpawner>();
        vehicleSpawnerInstance.VehiclePrefab = vehiclePrefab;
        vehicleSpawnerInstance.isTrain = true;
        vehicleSpawnerInstance.Train = train;
    }

    [Test]
    public void AIVehicleSpawnTest()
    {
        vehicleSpawnerInstance.Start();
        GameObject vehicle = GameObject.Find("Vehicle");

        Assert.IsTrue(vehicle != null);
    }

    [UnityTest]
    public IEnumerator AIVehicleTest()
    {
        vehicleSpawnerInstance.Start();
        GameObject vehicle = GameObject.Find("Vehicle");
        VehicleAI ai = vehicle.AddComponent<VehicleAI>();
        yield return new WaitForSeconds(ai.distanceToDestroy/(ai.speed * ai.step));
        Assert.IsTrue(vehicle == null);
    }

    [UnityTest]
    public IEnumerator VehicleStopperTest()
    {
        GameObject stopVehicleGO = new GameObject();
        stopVehicleGO.AddComponent<BoxCollider>();
        stopVehicleInstance = stopVehicleGO.AddComponent<StopVehicle>();

        Assert.IsFalse(stopVehicleInstance.stopped);

        yield return  new WaitForSeconds(stopVehicleInstance.wait);

        Assert.IsTrue(stopVehicleInstance.stopped);
    }
}
