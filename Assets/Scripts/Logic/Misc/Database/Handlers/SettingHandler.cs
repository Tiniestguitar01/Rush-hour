using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class SettingHandler : MonoBehaviour
{
    Database database;

    void Start()
    {
        database = InstanceCreator.GetDatabase();
    }

    public void AddSetting(Setting setting)
    {
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO `Setting` (name, value)  VALUES (@name, @value);";
                command.Parameters.AddWithValue("@name", setting.name);
                command.Parameters.AddWithValue("@value", setting.value);
                command.ExecuteNonQuery();

                command.CommandText = "INSERT OR REPLACE INTO `User_Setting` (user_id, setting_id) VALUES (@user_id, @setting_id);";
                command.Parameters.AddWithValue("@user_id", database.loggedInUser.id);
                command.Parameters.AddWithValue("@setting_id", setting.id);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public List<Setting> GetUserSettings(User user)
    {
        List<Setting> settings = new List<Setting>();
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name, value FROM `Setting` INNER JOIN `User_Setting` ON User_Setting.setting_id = Setting.id WHERE User_Setting.user_id = @user_id;";
                command.Parameters.AddWithValue("@user_id", database.loggedInUser.id);
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Setting setting = new Setting(int.Parse(reader["id"].ToString()),reader["name"].ToString(), reader["value"].ToString());
                        settings.Add(setting);
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }

        return settings;
    }
}
