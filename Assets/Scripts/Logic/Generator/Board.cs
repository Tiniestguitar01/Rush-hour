using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int size;

    public int[,] board;

    public List<Place> places = new List<Place>();
    GameData gameDataInstance;
    public int maxBoardSize = 7;
    SpawnGrid spawnGridInstance;

    private void Start()
    {
        spawnGridInstance = InstanceCreator.GetSpawnGrid();
        gameDataInstance = InstanceCreator.GetGameData();
        size = gameDataInstance.boardSize;
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        places.Clear();
        size = gameDataInstance.boardSize;
        Debug.Log("size: "+ size);
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
        return new Vector3((coordinate[0] * spawnGridInstance.distance) + ((maxBoardSize - size) * spawnGridInstance.offset), 0, (coordinate[1] * spawnGridInstance.distance) + ((maxBoardSize- size) * spawnGridInstance.offset));
    }

    public Place GetPlace(Vehicle vehicle, bool isForward)
    {
        List<Place> resultList = new List<Place>();
        if (isForward)
        {
            for (int i = 0; i < places.Count; i++)
            {
                if (places[i].placePosition[(int)vehicle.direction] < vehicle.startPosition[(int)vehicle.direction])
                {
                    if((places[i].placePosition[1 - (int)vehicle.direction] >= vehicle.startPosition[1 - (int)vehicle.direction] - 2) && places[i].placePosition[1 - (int)vehicle.direction] <= vehicle.startPosition[1 - (int)vehicle.direction] && places[i].size > 2 && places[i].direction != vehicle.direction)
                    {
                        resultList.Add(places[i]);
                    }

                    if ((places[i].placePosition[1 - (int)vehicle.direction] >= vehicle.startPosition[1 - (int)vehicle.direction] - 1) && places[i].placePosition[1 - (int)vehicle.direction] <= vehicle.startPosition[1 - (int)vehicle.direction] && places[i].direction != vehicle.direction)
                    {
                        resultList.Add(places[i]);
                    }

                    if (places[i].placePosition[1 - (int)vehicle.direction] == vehicle.startPosition[1 - (int)vehicle.direction])
                    {
                        resultList.Add(places[i]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < places.Count; i++)
            {
                if (places[i].placePosition[(int)vehicle.direction] > vehicle.startPosition[(int)vehicle.direction])
                {
                    if ((places[i].placePosition[1 - (int)vehicle.direction] >= vehicle.startPosition[1 - (int)vehicle.direction] - 2) && places[i].placePosition[1 - (int)vehicle.direction] <= vehicle.startPosition[1 - (int)vehicle.direction] && places[i].size > 2 && places[i].direction != vehicle.direction)
                    {
                        resultList.Add(places[i]);
                    }

                    if ((places[i].placePosition[1 - (int)vehicle.direction] >= vehicle.startPosition[1 - (int)vehicle.direction] - 1) && places[i].placePosition[1 - (int)vehicle.direction] <= vehicle.startPosition[1 - (int)vehicle.direction] && places[i].direction != vehicle.direction)
                    {
                        resultList.Add(places[i]);
                    }

                    if (places[i].placePosition[1 - (int)vehicle.direction] == vehicle.startPosition[1 - (int)vehicle.direction])
                    {
                        resultList.Add(places[i]);
                    }
                }
                
            }
        }

        if (resultList.Count > 0)
        {
            return resultList[Random.Range(0, resultList.Count - 1)];
        }
        else
        {
            return null;
        }
    }


    public void GetFreeSpaces()
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                if (board[x,z] == 0)
                {
                    if (x + 2 < size && board[x + 2, z] == 0 && board[x + 1, z] == 0)
                    {
                        places.Add(new Place(3, new int[] { x, z }, Direction.Vertical));
                    }

                    if (x + 1 < size && board[x + 1, z] == 0)
                    {
                        places.Add(new Place(2, new int[] { x, z }, Direction.Vertical));
                    }

                    if (z + 2 < size && board[x, z + 2] == 0 && board[x, z + 1] == 0)
                    {
                        places.Add(new Place(3, new int[] { x, z }, Direction.Horizontal));
                    }

                    if (z + 1 < size && board[x, z + 1] == 0)
                    {
                        places.Add(new Place(2, new int[] { x, z }, Direction.Horizontal));
                    }
                }
            }
        }
    }
}
