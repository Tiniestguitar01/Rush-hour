using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class User
{
    public int id;
    public string username;
    public string password;

    public User(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public User(int id,string username, string password)
    {
        this.id = id;
        this.username = username;
        this.password = password;
    }
}
