using System.Collections.Generic;
using UnityEngine;

public class ModifyBoard : MonoBehaviour
{
    public void InsertVehicle(Vehicle vehicle, int[,] board)
    {
        List<int[]> position = vehicle.GetPosition();
        for (int x = 0; x < position.Count; x++)
        {
            board[position[x][0], position[x][1]] = vehicle.id;
        }
    }

    public void RemoveVehicle(Vehicle vehicle, int[,] board)
    {
        List<int[]> position = vehicle.GetPosition();
        for (int x = 0; x < position.Count; x++)
        {
            board[position[x][0], position[x][1]] = 0;
        }
    }

    public void MoveVehicle(Vehicle vehicle, int[] position, int[,] board, bool fromSolver)
    {            
        RemoveVehicle(vehicle, board);
        vehicle.Move(position);
        InsertVehicle(vehicle, board);

        if(vehicle.id == 1 && vehicle.startPosition[0] == 0 && !fromSolver)
        {
            UIManager uIManager = InstanceCreator.GetUIManager();
            GameData gameData = InstanceCreator.GetGameData();
            Database database = InstanceCreator.GetDatabase();

            gameData.prevMoved = gameData.moved;
            gameData.prevTimer = gameData.GetTimeInString(gameData.timer);

            Result result = new Result((int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved);

            database.GetResultsByBoardSize(board.GetLength(0));

            if(database.results.Count >= (int)gameData.difficulty && database.results[(int)gameData.difficulty - 1] != null)
            {
                if (database.results[(int)gameData.difficulty - 1].moved > gameData.prevMoved)
                {
                    database.AddResult(new Result((int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
                }
                else if (database.results[(int)gameData.difficulty - 1].moved == gameData.prevMoved && database.results[(int)gameData.difficulty - 1].time > gameData.timer)
                {
                    database.AddResult(new Result((int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
                }

            }
            else
            {
                database.AddResult(new Result((int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
            }

            uIManager.SetMenuActive(Menu.GameOver);
        }
    }

}
