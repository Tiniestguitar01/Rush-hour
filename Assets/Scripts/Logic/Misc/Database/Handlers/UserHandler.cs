using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;

public class UserHandler : MonoBehaviour
{
    Database database;

    void Start()
    {
        database = InstanceCreator.GetDatabase();
    }

    public bool RegisterUser(User user)
    {
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            User checkUser = GetUserByUsername(user.username);
            if(checkUser != null)
            {
                connection.Close();
                return false;
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO `User` (username, password)  VALUES (@username, @password);";
                command.Parameters.AddWithValue("@username", user.username);
                command.Parameters.AddWithValue("@password", HashPassword(user.password));
                command.ExecuteNonQuery();

                connection.Close();

                database.loggedInUser = user;
                return true;
            }

        }

        return false;
    }

    public bool LoginUser(User user)
    {
        using (var connection = new SqliteConnection(database.databaseName))
        {
            connection.Open();

            User checkUser = GetUserByUsername(user.username);
            if (checkUser == null)
            {
                connection.Close();
                return false;
            }

            if(checkUser.username == user.username && checkUser.password == HashPassword(user.password))
            {
                connection.Close();
                database.loggedInUser = user;
                return true;
            }

        }

        return false;
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
