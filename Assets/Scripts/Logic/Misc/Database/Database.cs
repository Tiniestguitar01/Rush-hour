using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using Codice.Client.Common;
using UnityEngine.Assertions;

public class Database : MonoBehaviour
{
    public string databaseName = "URI=file:RushHour.db";
    public User loggedInUser;

    [HideInInspector]
    public UserHandler userHandler;
    [HideInInspector]
    public ResultHandler resultHandler;
    [HideInInspector]
    public SettingHandler settingHandler;

    void Awake()
    {
        CreateDatabase();
        userHandler = GetComponent<UserHandler>();
        resultHandler = GetComponent<ResultHandler>();
        settingHandler = GetComponent<SettingHandler>();
    }

    public void CreateDatabase()
    {
        using (var connection = new SqliteConnection(databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                string resultTable = "CREATE TABLE IF NOT EXISTS `Result` (`id` INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,`difficulty` INT,`board_size` INT,`time` FLOAT,`moved` INT);";
                string userTable = "CREATE TABLE IF NOT EXISTS `User` (`id` INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,`username` VARCHAR(255),`password` VARCHAR(255));";
                string settingTable = "CREATE TABLE IF NOT EXISTS `Setting` (`id` INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,`name` VARCHAR(255),`password` VARCHAR(255));";
                string userResultTable = "CREATE TABLE IF NOT EXISTS `User_Result` (`user_id` INT,`result_id` INT,PRIMARY KEY (`user_id`,`result_id`));";
                string userSettingTable = "CREATE TABLE IF NOT EXISTS `User_Setting` (`user_id` INT,`setting_id` INT,PRIMARY KEY (`user_id`,`setting_id`));";
                string insertGuest = "INSERT OR REPLACE INTO `User` (username, password) VALUES ('guest', 'pass');";
                command.CommandText = resultTable + userTable + settingTable + userResultTable + userSettingTable + insertGuest;
                command.ExecuteNonQuery();
                loggedInUser = new User(1,"guest", "pass");
            }

            connection.Close();
        }
    }
}
