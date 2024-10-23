using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;

public class Error
{
    public bool isSuccessful;
    public string message;
    public Error(bool isSuccessful, string message)
    {
        this.isSuccessful = isSuccessful;
        this.message = message;
    }
}

public class UserHandler : MonoBehaviour
{
    Database database;
    Settings settings;

    void Start()
    {
        database = InstanceCreator.GetDatabase();
        settings = InstanceCreator.GetSettings();
    }

    public Error RegisterUser(User user)
    {
        if (user.username.Length < 5)
        {
            return new Error(false, "Username is too short!\nUse at least 5 character!");
        }

        if (user.password.Length < 10)
        {
            return new Error(false, "Password is too short!\nUse at least 10 character!");
        }

        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            User checkUser = GetUserByUsername(user.username);
            if(checkUser != null)
            {
                connection.Close();
                return new Error(false, "User already exists!");
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO `User` (username, password)  VALUES (@username, @password);";
                command.Parameters.AddWithValue("@username", user.username);
                command.Parameters.AddWithValue("@password", HashPassword(user.password));
                command.ExecuteNonQuery();

                command.CommandText = "SELECT COUNT(*) AS count FROM `User`;";
                command.ExecuteNonQuery();
                int id = 0;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = int.Parse(reader["count"].ToString());
                    }
                    reader.Close();
                }

                connection.Close();

                database.loggedInUser = user;
                database.loggedInUser.id = id;
                return new Error(true, "Registration was successful!");
            }

        }
    }

    public Error LoginUser(User user)
    {
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            User checkUser = GetUserByUsername(user.username);
            if (checkUser == null)
            {
                connection.Close();
                return new Error(false, "Cannot find a user with this name!");
            }

            if(checkUser.username == user.username && checkUser.password == HashPassword(user.password))
            {
                connection.Close();
                database.loggedInUser = checkUser;
                settings.GetSettings();
                return new Error(true, "You are logged in!");
            }
            else
            {
                return new Error(false, "Incorrect username or password!");
            }
        }
    }

    public void LogoutUser()
    {
        database.loggedInUser = new User(1,"guest", "pass");
    }

    public User GetUserByUsername(string username)
    {
        User user = null;

        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `User` WHERE username = '" + username + "';";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log(int.Parse(reader["id"].ToString()));
                        user = new User(int.Parse(reader["id"].ToString()), reader["username"].ToString(), reader["password"].ToString());
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }

        return user;
    }

    string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
