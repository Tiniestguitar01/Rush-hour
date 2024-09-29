using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
        
    public GameObject cell;
    float distance = 3.5f;

    public List<GameObject> instantiatedCells;
    private void Start()
    {
        instantiatedCells = new List<GameObject>();
    }
    public void Spawn()
    {
        for (float x = 0; x < InstanceCreator.GetBoard().size * distance; x += distance)
        {
            for (float z = 0; z < InstanceCreator.GetBoard().size * distance; z += distance)
            {
                instantiatedCells.Add(Instantiate(cell, new Vector3(x, 0, z), Quaternion.identity));
            }
        }
    }
}
