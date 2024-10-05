using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class SettingHandler : MonoBehaviour
{
    Database database;

    private void Awake()
    {
        database = InstanceCreator.GetDatabase();
    }

    void Start()
    {
        GetSettings();
    }

    public void AddSetting(Setting setting)
    {
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO `Setting` (name, value,user_id)  VALUES (@name, @value, @user_id);";
                command.Parameters.AddWithValue("@name", setting.name);
                command.Parameters.AddWithValue("@value", setting.value);
                command.Parameters.AddWithValue("@user_id", database.loggedInUser.id);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public Dictionary<string,string> GetUserSettings(User user)
    {
        Dictionary < string,string > settings = new Dictionary<string, string>();
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name, value FROM `Setting` WHERE user_id = @user_id;";
                command.Parameters.AddWithValue("@user_id", database.loggedInUser.id);
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        settings[reader["name"].ToString()] = reader["value"].ToString();
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }

        return settings;
    }

    public Setting GetSettings()
    {
        Setting setting = null;

        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `Setting`;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    Debug.Log("Setting");
                    while (reader.Read())
                    {
                        Debug.Log("Setting: {" + reader["user_id"].ToString() + ", " + reader["name"].ToString() + ", " + reader["value"].ToString() + "}");
                        setting = new Setting(reader["name"].ToString(), reader["value"].ToString(), int.Parse(reader["user_id"].ToString()));
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }

        return setting;
    }
}
