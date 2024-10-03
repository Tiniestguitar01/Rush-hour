using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Result
{
    public int id;
    public int difficulty;
    public int boardSize;
    public float time;
    public int moved;

    public Result(int difficulty, int boardSize, float time, int moved)
    {
        this.difficulty = difficulty;
        this.boardSize = boardSize;
        this.time = time;
        this.moved = moved;
    }

    public Result(int id,int difficulty, int boardSize, float time, int moved)
    {
        this.id = id;
        this.difficulty = difficulty;
        this.boardSize = boardSize;
        this.time = time;
        this.moved = moved;
    }
}