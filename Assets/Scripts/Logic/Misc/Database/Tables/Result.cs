using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result
{
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
}