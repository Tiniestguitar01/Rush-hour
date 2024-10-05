using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserResultDTO
{
    public string username;
    public int moved;
    public float time;

    public UserResultDTO(string username,int moved,float time)
    {
        this.username = username;
        this.moved = moved;
        this.time = time;
    }
}
