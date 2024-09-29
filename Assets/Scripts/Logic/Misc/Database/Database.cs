using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class Database : MonoBehaviour
{
    string databaseName = "URI=file:RushHour.db";
    public List<Result> results = new List<Result>();

    void Awake()
    {
        CreateDatabase();
        GetResultsByBoardSize(6);
    }

    public void CreateDatabase()
    {
        using (var connection = new SqliteConnection(databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS `Result` (`difficulty` INT,`board_size` INT,`time` FLOAT,`moved` INT,PRIMARY KEY (`difficulty`,`board_size`));";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void AddResult(Result result)
    {
        using (var connection = new SqliteConnection(databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO `Result` (difficulty, board_size, time, moved)  VALUES ('" + result.difficulty + "','" + result.boardSize + "','" + result.time + "', '" + result.moved + "' );";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void GetResultsByBoardSize(int size)
    {
        results.Clear();
        using (var connection = new SqliteConnection(databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `Result` WHERE board_size = '" + size + "';";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Result result = new Result(int.Parse(reader["difficulty"].ToString()), int.Parse(reader["board_size"].ToString()), float.Parse(reader["time"].ToString()), int.Parse(reader["moved"].ToString()));
                        results.Add(result);
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }
    }
}
