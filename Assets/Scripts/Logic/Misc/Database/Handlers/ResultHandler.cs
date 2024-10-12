using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    Database database;

    void Start()
    {
        database = InstanceCreator.GetDatabase();
        GetResults();
    }

    public void AddResult(Result result)
    {
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {

                command.CommandText = "INSERT OR REPLACE INTO `Result` (user_id, difficulty, board_size, time, moved)  VALUES (@user_id, @difficulty, @board_size, @time, @moved);";
                command.Parameters.AddWithValue("@user_id", database.loggedInUser.id);
                command.Parameters.AddWithValue("@difficulty", result.difficulty);
                command.Parameters.AddWithValue("@board_size", result.boardSize);
                command.Parameters.AddWithValue("@time", result.time);
                command.Parameters.AddWithValue("@moved", result.moved);
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
                command.CommandText = "SELECT * FROM `Result` WHERE board_size = @size AND Result.user_id = @user_id;";
                command.Parameters.AddWithValue("@user_id", database.loggedInUser.id);
                command.Parameters.AddWithValue("@size", size);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Result result = new Result(int.Parse(reader["user_id"].ToString()),int.Parse(reader["difficulty"].ToString()), int.Parse(reader["board_size"].ToString()), float.Parse(reader["time"].ToString()), int.Parse(reader["moved"].ToString()));
                        results.Add(result);
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }

        return results;
    }

    public List<UserResultDTO> GetResultsByBoardSizeAndDifficulty(int difficulty,int size)
    {
        List<UserResultDTO> results = new List<UserResultDTO>();
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT User.username, moved, time FROM `Result` LEFT JOIN `User` ON User.id = Result.user_id WHERE board_size = @size AND difficulty = @difficulty ORDER BY moved ASC,time ASC LIMIT 50;";
                command.Parameters.AddWithValue("@size", size);
                command.Parameters.AddWithValue("@difficulty", difficulty);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserResultDTO result = new UserResultDTO(reader["username"].ToString(), int.Parse(reader["moved"].ToString()), float.Parse(reader["time"].ToString()));
                        results.Add(result);
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }

        return results;
    }

    public List<Result> GetResults()
    {
        List<Result> results = new List<Result>();
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `Result`;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    Debug.Log("Result");
                    while (reader.Read())
                    {
                        Debug.Log("Result: {" + reader["user_id"].ToString() + ", " + reader["difficulty"].ToString() + ", " + reader["board_size"].ToString() + ", " + reader["time"].ToString() + ", "  + reader["moved"].ToString() +  "}");
                        Result result = new Result(int.Parse(reader["user_id"].ToString()), int.Parse(reader["difficulty"].ToString()), int.Parse(reader["board_size"].ToString()), float.Parse(reader["time"].ToString()), int.Parse(reader["moved"].ToString()));
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
