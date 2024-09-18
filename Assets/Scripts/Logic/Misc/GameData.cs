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
        public float timer = 0;

        public bool timerStarted = false;

        public Difficulty difficulty;

        void Awake()
        {
            difficulty = Difficulty.Beginner;
            Instance = this;
        }

        void Update()
        {
            if (UIManager.Instance.state == (int)Menu.Game)
            {
                if (timerStarted)
                {
                    timer += Time.deltaTime;
                }

                string minute = Mathf.Floor((timer / 60)).ToString("00");
                string second = (timer % 59).ToString("00");
                UIManager.Instance.TimerText.text = minute + ":" + second;

                UIManager.Instance.MovedText.text = "Moved: " + moved;
            }
            else
            {
                moved = 0;
                timer = 0;
            }

        }

        public void StartTimer()
        {
            timerStarted = true;
            timer = 0;
        }

        public void StopTimer()
        {
            timerStarted = false;
        }
    }
