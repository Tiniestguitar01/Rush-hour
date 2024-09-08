using UnityEngine;

public class SpawnGrid : MonoBehaviour
{

    public GameObject cell;
    public int distance = 3;

    public static SpawnGrid Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn()
    {
        for (int x = 0; x < Board.Instance.size * distance; x += distance)
        {
            for (int z = 0; z < Board.Instance.size * distance; z += distance)
            {
                Instantiate(cell, new Vector3 (x,0,z), Quaternion.identity);
            }
        }
    }
}
