using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public enum Menu
{
    Menu = 0,
    Game = 1,
    Difficulty = 2,
    Pause = 3,
    Loading = 4,
    Options = 5,
    GameOver = 6,
    Login = 7,
    Register = 8,
    Leaderboard = 9,
}
public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    public List<GameObject> menus;

    public int state = 0; //0=menu,1=game
    public Menu previousMenu;

    [HideInInspector]
    public GameUI gameUI;
    [HideInInspector]
    public PauseUI pauseUI;
    [HideInInspector]
    public DifficultyUI difficultyUI;
    [HideInInspector]
    public GameOverUI gameOverUI;
    [HideInInspector]
    public UserHandleUI userHandleUI;
    [HideInInspector]
    public LeaderboardUI leaderboardUI;

    GameData gameDataInstance;
    Settings settingsInstance;
    Database databaseInstance;

    private void Start()
    {
        gameUI = GetComponent<GameUI>();
        difficultyUI = GetComponent<DifficultyUI>();
        gameOverUI = GetComponent<GameOverUI>();
        userHandleUI = GetComponent<UserHandleUI>();
        leaderboardUI = GetComponent<LeaderboardUI>();
        pauseUI = GetComponent<PauseUI>();
        SetMenuActive(Menu.Menu);
    }

    public void SetMenuActive(Menu menuCode)
    {
        previousMenu = GetMenuActive();
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        menus[(int)menuCode].SetActive(true);
        state = (int)menuCode;

        if (menuCode == Menu.Pause)
        {
            pauseUI.paused = true;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            pauseUI.paused = false;
        }

        int i = 0;
        while(menus[(int)menuCode].transform.GetChild(i).GetComponent<Button>() == null)
        {
            i++;
        }
        if(menus[(int)menuCode].transform.GetChild(i).gameObject.GetComponent<Button>() != null)
        {
            Debug.Log(i);
            EventSystem.current.SetSelectedGameObject(menus[(int)menuCode].transform.GetChild(i).gameObject, new BaseEventData(EventSystem.current));
        }
    }

    public Menu GetMenuActive()
    {
        return (Menu)state;
    }

    public void StartToDifficulty()
    {
        difficultyUI.BoardSizeSliderChange();
        SetMenuActive(Menu.Difficulty);
    }

    public void BackToMenu()
    {
        SetMenuActive(Menu.Menu);
    }

    public void BackToPreviousMenu()
    {
        SetMenuActive(previousMenu);
    }

    public void StartToOptions()
    {
        SetMenuActive(Menu.Options);
    }

    public void StartToLogin()
    {
        SetMenuActive(Menu.Login);
    }

    public void StartToRegister()
    {
        SetMenuActive(Menu.Register);
    }

    public void StartToLeaderboard()
    {
        leaderboardUI.GetLeaderBoard();
        SetMenuActive(Menu.Leaderboard);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
