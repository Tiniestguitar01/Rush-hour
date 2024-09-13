using System;
using UnityEngine;

[Serializable]
public enum Difficulty
{
    Beginner = 10,
    Intermediate = 25,
    Advanced = 50,
    Expert = 75
}

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int moved = 0;

    public Difficulty difficulty;

    void Awake()
    {
        difficulty = Difficulty.Beginner;
        Instance = this;
    }

    void Update()
    {
        string minute = Mathf.Floor((Time.time / 60)).ToString("00");
        string second = (Time.time % 59).ToString("00");
        UIManager.Instance.TimerText.text = minute + ":" + second;

        UIManager.Instance.MovedText.text = "Moved: " + moved;
    }
}
