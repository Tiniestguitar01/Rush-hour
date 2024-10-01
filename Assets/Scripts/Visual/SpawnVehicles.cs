using System.Collections.Generic;
using UnityEngine;

public class SpawnVehicles : MonoBehaviour
{
    public List<GameObject> cars = new List<GameObject>();
    public List<GameObject> trucks = new List<GameObject>();

    public List<GameObject> vehicleGOs = new List<GameObject>();

    public void Spawn()
    {
        List<Vehicle> vehicles = InstanceCreator.GetPuzzleGenerator().vehicles;

        for (int id = 1; id < vehicles.Count + 1; id++)
        {
            Vehicle vehicle = vehicles[id - 1];
            GameObject holder;
            GameObject clone;

            holder = new GameObject();
            holder.name = id.ToString();

            vehicleGOs.Add(holder);

            if (vehicles[id - 1].id == 1)
            {
                clone = Instantiate(cars[0]);
            }
            else if (vehicle.size == 2 && vehicles[id - 1].id != 1)
            {
                int random = Random.Range(1, cars.Count - 1);
                clone = Instantiate(cars[random]);
            }
            else
            {
                int random = Random.Range(0, trucks.Count - 1);
                clone = Instantiate(trucks[random]);
            }

            clone.name = id.ToString();
            clone.transform.parent = holder.transform;
            clone.AddComponent<VehicleComponent>().Init(vehicle.id, vehicle.size, vehicle.startPosition, vehicle.direction);
            holder.transform.position = new Vector3(vehicle.startPosition[0] * 3.5f, (0.55f * vehicle.size), vehicle.startPosition[1] * 3.5f);
            clone.transform.localPosition = -new Vector3((int)vehicle.direction == 0 ? -1 : 0, 0, (int)vehicle.direction == 1 ? -1 : 0) * vehicle.size * 1f;
            clone.transform.rotation = Quaternion.Euler(0, -90 * (int)vehicle.direction + 180, 0);
        }
    }
}
