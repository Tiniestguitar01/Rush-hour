using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject VehiclePrefab;

    public float minWait = 5f;
    public float maxWait = 10f;

    SpawnVehicles spawnVehiclesInstance;
    void Start()
    {
        spawnVehiclesInstance = InstanceCreator.GetSpawnVehicles();
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        GameObject vehicleParent = Instantiate(VehiclePrefab, transform.position, Quaternion.identity);
        int random = Random.Range(0, 100); 
        GameObject vehicle;
        float size = 0;
        if (random > 50)
        {
            vehicle = Instantiate(spawnVehiclesInstance.cars[Random.Range(0, spawnVehiclesInstance.cars.Count-1)], vehicleParent.transform.position, Quaternion.identity);
            size = 1.8f;
        }
        else
        {
            vehicle = Instantiate(spawnVehiclesInstance.trucks[Random.Range(0, spawnVehiclesInstance.trucks.Count - 1)], vehicleParent.transform.position, Quaternion.identity);
            size = 3.55f;
        }

        vehicle.transform.parent = vehicleParent.transform;
        vehicle.transform.position = new Vector3(vehicleParent.transform.position.x, (0.50f * size), vehicleParent.transform.position.z);

        vehicleParent.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(Random.Range(minWait,maxWait));
        StartCoroutine(Spawn());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(5f, 5f, 5f));
    }
}
