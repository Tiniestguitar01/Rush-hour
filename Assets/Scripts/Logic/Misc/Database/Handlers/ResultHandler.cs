using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class ResultHandler : MonoBehaviour
{
    Database database;

    void Start()
    {
        database = InstanceCreator.GetDatabase();
    }

    public void AddResult(Result result)
    {
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO `Result` (difficulty, board_size, time, moved)  VALUES (@difficulty, @board_size, @time, @moved);";
                command.Parameters.AddWithValue("@difficulty", result.difficulty);
                command.Parameters.AddWithValue("@board_size", result.boardSize);
                command.Parameters.AddWithValue("@time", result.time);
                command.Parameters.AddWithValue("@moved", result.moved);
                command.ExecuteNonQuery();

                command.CommandText = "INSERT OR REPLACE INTO `User_Result` (user_id, result_id)  VALUES (@user_id, @result_id);";
                command.Parameters.AddWithValue("@user_id", database.loggedInUser.id);
                command.Parameters.AddWithValue("@result_id", result.id);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public List<Result> GetResultsByBoardSize(int size)
    {
        List<Result> results = new List<Result>();
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `Result` INNER JOIN `User_Result` ON User_Result.result_id = Result.id WHERE board_size = @size AND User_Result.user_id = @user_id;";
                command.Parameters.AddWithValue("@user_id", database.loggedInUser.id);
                command.Parameters.AddWithValue("@size", size);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Result result = new Result(int.Parse(reader["id"].ToString()),int.Parse(reader["difficulty"].ToString()), int.Parse(reader["board_size"].ToString()), float.Parse(reader["time"].ToString()), int.Parse(reader["moved"].ToString()));
                        results.Add(result);
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }

        return results;
    }

    public List<Result> GetResultsByBoardSizeAndDifficulty(int difficulty,int size)
    {
        List<Result> results = new List<Result>();
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `Result` WHERE board_size = @size AND difficulty = @difficulty ORDER BY moved DESC,time DESC;";
                command.Parameters.AddWithValue("@size", size);
                command.Parameters.AddWithValue("@difficulty", difficulty);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Result result = new Result(int.Parse(reader["id"].ToString()), int.Parse(reader["difficulty"].ToString()), int.Parse(reader["board_size"].ToString()), float.Parse(reader["time"].ToString()), int.Parse(reader["moved"].ToString()));
                        results.Add(result);
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }

        return results;
    }
}
