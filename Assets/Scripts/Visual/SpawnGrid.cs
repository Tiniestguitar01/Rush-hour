using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
        
    public GameObject cell;
    public float distance = 3.5f;
    public float offset = 2f;

    Board boardInstance;

    public Material goalCellMaterial;

    public List<GameObject> instantiatedCells;
    public void Start()
    {
        boardInstance = InstanceCreator.GetBoard();
        instantiatedCells = new List<GameObject>();
    }
    public void Spawn()
    {
        for (float x = (boardInstance.maxBoardSize - boardInstance.size) * offset; x < boardInstance.size * distance + (boardInstance.maxBoardSize - boardInstance.size) * offset; x += distance)
        {
            for (float z = (boardInstance.maxBoardSize - boardInstance.size) * offset; z < boardInstance.size * distance + (boardInstance.maxBoardSize - boardInstance.size) * offset; z += distance)
            {
                GameObject cellGO = Instantiate(cell, new Vector3(x, 0, z), Quaternion.identity);
                if (x == (boardInstance.maxBoardSize - boardInstance.size) * offset && z == ((boardInstance.maxBoardSize - boardInstance.size) * offset) + (2 * distance))
                {
                    cellGO.GetComponent<Renderer>().material = goalCellMaterial;
                }
                instantiatedCells.Add(cellGO);
            }
        }
    }
}
