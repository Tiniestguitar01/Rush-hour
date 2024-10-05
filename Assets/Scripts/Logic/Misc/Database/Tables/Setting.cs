using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting
{
    public int userId;
    public string name;
    public string value;

    public Setting(string name, string value,int userId)
    {
        this.name = name;
        this.value = value;
        this.userId = userId;
    }
}
