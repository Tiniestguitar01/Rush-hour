using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting
{
    public int id;
    public string name;
    public string value;

    public Setting(string name, string value)
    {
        this.name = name;
        this.value = value;
    }

    public Setting(int id,string name, string value)
    {
        this.id = id;
        this.name = name;
        this.value = value;
    }
}
