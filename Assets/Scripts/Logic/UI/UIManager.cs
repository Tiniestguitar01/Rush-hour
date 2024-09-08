using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Menu
{
    Menu = 0,
    Game = 1,
    Difficulty = 2,
    Options = 3
}

public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    public List<GameObject> menus;

    [Header("GameUI")]

    public TMP_Text TimerText;
    public TMP_Text MovedText;

    public static UIManager Instance;

    public int state = 0;

    void Start()
    {
        Instance = this;
    }

    public void SetMenuActive(Menu menuCode)
    {
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }

        menus[(int)menuCode].SetActive(true);
        state = (int)menuCode;
    }

    public void StartToDifficulty()
    {
        SetMenuActive(Menu.Difficulty);
    }

    public void StartGame()
    {
        SetMenuActive(Menu.Game);
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
}
