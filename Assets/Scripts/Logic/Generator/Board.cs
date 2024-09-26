using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int size;

    public int[,] board;

    public List<Place> places = new List<Place>();

    void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        board = new int[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                board[x, z] = 0;

                if (x + 2 < size && z != 2)
                {
                    places.Add(new Place(3, new int[] { x, z }, Direction.Vertical));
                }

                if (x + 1 < size && z != 2)
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
        return new Vector3(coordinate[0] * 3.5f, 0, coordinate[1] * 3.5f);
    }

    public Place GetRandomPlaceByCoordinate(int[] position)
    {
        for (int i = 0; i < places.Count; i++ )
        {
            if (places[i].placePosition[0] == position[0] && places[i].placePosition[1] == position[1])
            {
                return places[i];
            }
        }

        if(places.Count > 0)
        {
            return places[Random.Range(0,places.Count)];
        }
        else
        {
            return null;
        }
    }
}
