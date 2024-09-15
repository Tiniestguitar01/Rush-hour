using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;

    public int size;

    public int[,] board;

    public List<Place> places = new List<Place>();

    void Awake()
    {
        Instance = this;
        GenerateBoard();
    }

    void GenerateBoard()
    {
        board = new int[size, size];

        for (int x = 0; x < size; x++) 
        {
            for (int z = 0; z < size; z++)
            {
                board[x, z] = 0;

                if (x + 2 < size)
                {
                    places.Add(new Place(3, new int[] { x, z }, Direction.Vertical));
                }

                if (x + 1 < size)
                {
                    places.Add(new Place(2, new int[] { x, z }, Direction.Vertical));
                }

                if (z + 2 < size)
                {

                    places.Add(new Place(3, new int[] { x, z }, Direction.Horizontal));
                }

                if (z + 1 < size)
                {
                    places.Add(new Place(2, new int[] { x, z }, Direction.Horizontal));
                }
            }
        }
    }

    public Vector3 BoardCoordinateToWordSpace(int[] coordinate)
    {
        return new Vector3(coordinate[0] * 3, 0, coordinate[1] * 3);
    }

    public int[] WordSpaceToBoardCoordinateTo(Vector3 coordinate)
    {
        return new int[] { (int)coordinate.x / 3, (int)coordinate.z / 3 };
    }
}
