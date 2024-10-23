using System.Collections.Generic;
using UnityEngine;

public class ModifyBoard : MonoBehaviour
{
    UIManager uIManager;
    GameData gameData;
    Database database;
    public void Start()
    {
        uIManager = InstanceCreator.GetUIManager();
        gameData = InstanceCreator.GetGameData();
        database = InstanceCreator.GetDatabase();
    }
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

        if (vehicle.id == 1 && vehicle.startPosition[0] == 0 && gameData != null && gameData.state == 1)
        {
            if (!fromSolver)
            {
                gameData.prevMoved = gameData.moved + 1;
                gameData.prevTimer = gameData.GetTimeInString(gameData.timer);

                uIManager.gameOverUI.Title.text = "Congratulations!";
                uIManager.gameOverUI.TimeText.text = "Time: " + gameData.prevTimer;
                uIManager.gameOverUI.MovesText.text = "Moved: " + gameData.prevMoved;

                Result result = new Result(database.loggedInUser.id, (int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved);

                List<Result> results = database.resultHandler.GetResultsByBoardSize(board.GetLength(0));

                if (results.Count >= (int)gameData.difficulty && results[(int)gameData.difficulty - 1] != null)
                {
                    if (results[(int)gameData.difficulty - 1].moved > gameData.prevMoved)
                    {
                        database.resultHandler.AddResult(new Result(results[(int)gameData.difficulty - 1].userId, (int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
                    }
                    else if (results[(int)gameData.difficulty - 1].moved == gameData.prevMoved && results[(int)gameData.difficulty - 1].time > gameData.timer)
                    {
                        database.resultHandler.AddResult(new Result(results[(int)gameData.difficulty - 1].userId, (int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
                    }

                }
                else
                {
                    database.resultHandler.AddResult(new Result(database.loggedInUser.id, (int)gameData.difficulty, board.GetLength(0), gameData.timer, gameData.prevMoved));
                }
            }
            else
            {
                uIManager.gameOverUI.Title.text = "Next time will be better!\nTry another puzzle!";
                uIManager.gameOverUI.TimeText.text = "";
                uIManager.gameOverUI.MovesText.text = "";
            }

            uIManager.SetMenuActive(Menu.GameOver);
        }
    }

}
