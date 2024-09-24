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

    [Test]
    public void AIVehicleSpawnTest()
    {
        GameObject VehicleSpawnerGO = new GameObject("Spawner");
        VehicleSpawner vehicleSpawnerInstance = VehicleSpawnerGO.AddComponent<VehicleSpawner>();
        vehicleSpawnerInstance.vehicles.Add(car);
        vehicleSpawnerInstance.Spawn();
    }

    [Test]
    public void AIVehicleTest()
    {
        GameObject VehicleSpawnerGO = new GameObject("Spawner");
        VehicleSpawner vehicleSpawnerInstance = VehicleSpawnerGO.AddComponent<VehicleSpawner>();
        vehicleSpawnerInstance.vehicles.Add(car);
        vehicleSpawnerInstance.Spawn();
    }

    [UnityTest]
    public IEnumerator VehicleStopperTest()
    {
        GameObject stopVehicleGO = new GameObject("Stopper");
        StopVehicle stopVehicleInstance = stopVehicleGO.AddComponent<StopVehicle>();

        stopVehicleInstance.InstantiateRed(redPrefab);
        stopVehicleInstance.Stop();

        Assert.IsFalse(stopVehicleInstance.stopped);
        Assert.IsFalse(stopVehicleInstance.red.activeSelf);

        yield return  new WaitForSeconds(stopVehicleInstance.wait);

        Assert.IsTrue(stopVehicleInstance.stopped);
        Assert.IsTrue(stopVehicleInstance.red.activeSelf);
    }
}
