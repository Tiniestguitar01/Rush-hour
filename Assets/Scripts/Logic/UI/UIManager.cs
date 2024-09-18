using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Menu
{
    Menu = 0,
    Game = 1,
    Difficulty = 2,
    Pause = 3,
    GameOver = 4,
}
public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    public List<GameObject> menus;
    [Header("GameUI")]
    public TMP_Text TimerText;
    public TMP_Text MovedText;
    public static UIManager Instance;
    public int state = 0; //0=menu,1=game
    public bool paused = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(state == 1)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                HandlePause();
            }
        }
    }

    public void SetMenuActive(Menu menuCode)
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        menus[(int)menuCode].SetActive(true);
        state = (int)menuCode;

        Time.timeScale = 1f;
        paused = false;
    }

    public void StartToDifficulty()
    {
        SetMenuActive(Menu.Difficulty);
    }

    public void StartGame()
    {
        PuzzleGenerator.Instance.GeneratePuzzle();
        SetMenuActive(Menu.Game);
        GameData.Instance.StartTimer();
    }

    public void BackToMenu()
    {
        SetMenuActive(Menu.Menu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetDifficulty(int difficulty)
    {
        GameData.Instance.difficulty = (Difficulty)difficulty;
        StartGame();
    }

    public void HandlePause()
    {
        if(paused == false)
        {
            SetMenuActive(Menu.Pause);
            paused = true;
            Time.timeScale = 0f;
        }
        else
        {
            SetMenuActive(Menu.Game);
        }
    }
}
