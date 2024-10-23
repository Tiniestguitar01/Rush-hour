using System;
using UnityEngine;


[Serializable]
public enum Difficulty
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4
}

public class GameData : MonoBehaviour
{

    public int moved = 0;
    public float timer = 0;

    public int prevMoved = 0;
    public string prevTimer = "";

    public bool timerStarted = false;

    public Difficulty difficulty;

    public int boardSize = 6;

    public int state = 0;

    UIManager uiManagerInstance;

    public void Start()
    {
        uiManagerInstance = InstanceCreator.GetUIManager();
        difficulty = Difficulty.Beginner;
    }

    void Update()
    {
        if (state == (int)Menu.Game || state == (int)Menu.Pause)
        {
            if (timerStarted)
            {
                timer += Time.deltaTime;
            }

            uiManagerInstance.gameUI.TimerText.text = GetTimeInString(timer);

            uiManagerInstance.gameUI.MovedText.text = "Moved: " + moved;
        }
        else
        {
            moved = 0;
            timer = 0;
        }
    }

    public void StartTimer(bool fromZero = true)
    {
        timerStarted = true;
        if (fromZero)
        {
            timer = 0;
        }
    }

    public void StopTimer()
    {
        timerStarted = false;
    }

    public string GetTimeInString(float timer)
    {
        string minute = Mathf.Floor((timer / 59)).ToString("00");
        string second = (timer % 59).ToString("00");
        return minute + ":" + second;
    }
}
