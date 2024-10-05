using System.Collections.Generic;
using UnityEngine;

public class ModifyBoard : MonoBehaviour
{
    public void InsertVehicle(Vehicle vehicle, int[,] board)
    {
        Debug.Log(vehicle.ToString());
        List<int[]> position = vehicle.GetPosition();
        for (int x = 0; x < position.Count; x++)
        {
            Debug.Log("insert point:" +  position[x][0] + ", " + position[x][1]);
            board[position[x][0], position[x][1]] = vehicle.id;
        }
    }

    public void RemoveVehicle(Vehicle vehicle, int[,] board)
    {
        Debug.Log(vehicle.ToString());
        List<int[]> position = vehicle.GetPosition();
        for (int x = 0; x < position.Count; x++)
        {
            Debug.Log("delete point:" + position[x][0] + ", " + position[x][1]);
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

            gameData.prevMoved = gameData.moved + 1;
            gameData.prevTimer = gameData.GetTimeInString(gameData.timer);

            Result result = new Result((int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved);

            List<Result> results = database.resultHandler.GetResultsByBoardSize(board.GetLength(0));

            if(results.Count >= (int)gameData.difficulty && results[(int)gameData.difficulty - 1] != null)
            {
                if (results[(int)gameData.difficulty - 1].moved > gameData.prevMoved)
                {
                    database.resultHandler.AddResult(new Result(results[(int)gameData.difficulty - 1].id, (int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
                }
                else if (results[(int)gameData.difficulty - 1].moved == gameData.prevMoved && results[(int)gameData.difficulty - 1].time > gameData.timer)
                {
                    Debug.Log("jeeeeeeeeeeee time");
                    database.resultHandler.AddResult(new Result(results[(int)gameData.difficulty - 1].id, (int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
                }

            }
            else
            {
                database.resultHandler.AddResult(new Result((int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
            }

            uIManager.SetMenuActive(Menu.GameOver);
        }
    }

}
