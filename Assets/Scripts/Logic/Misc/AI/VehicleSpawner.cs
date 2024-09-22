using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject VehiclePrefab;

    public List<GameObject> vehicles;

    public float minWait = 5f;
    public float maxWait = 10f;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        GameObject vehicleParent = Instantiate(VehiclePrefab, transform.position, Quaternion.identity);
        GameObject vehicle = Instantiate(vehicles[Random.Range(0,vehicles.Count-1)], vehicleParent.transform.position, Quaternion.identity);

        vehicle.transform.parent = vehicleParent.transform;

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
