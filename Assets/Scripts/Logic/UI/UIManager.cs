using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.PackageManager.Requests;

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

    [Header("GameUI")]
    public TMP_Text TimerText;
    public TMP_Text MovedText;
    public int state = 0; //0=menu,1=game
    public bool paused = false;
    public Menu previousMenu;

    [Header("DifficultyUI")]
    public UnityEngine.UI.Slider boardSizeSlider;
    public List<TMP_Text> PRTexts;

    [Header("GameOverUI")]
    public TMP_Text TimeText;
    public TMP_Text MovesText;

    [Header("LoginUI")]
    public TMP_InputField loginUserNameInput;
    public TMP_InputField loginPasswordInput;

    [Header("RegisterUI")]
    public TMP_InputField registerUserNameInput;
    public TMP_InputField registerPasswordInput;

    [Header("LeaderboardUI")]
    public UnityEngine.UI.Slider leaderboardBoardSizeSlider;
    public TMP_Dropdown difficultyDropdown;
    public Transform content;
    public GameObject Record;
    List<GameObject> instantiatedRecords;

    [Header("MenuUI")]
    public GameObject LoginButton;
    public GameObject LogoutButton;


    GameData gameDataInstance;
    Settings settingsInstance;
    Database databaseInstance;

    private void Start()
    {
        instantiatedRecords = new List<GameObject>();
        gameDataInstance = InstanceCreator.GetGameData();
        databaseInstance = InstanceCreator.GetDatabase();
        SetMenuActive(Menu.Menu);
        gameDataInstance.boardSize = (int)boardSizeSlider.value;
        boardSizeSlider.onValueChanged.AddListener(delegate { BoardSizeSliderChange(); });
    }

    void Update()
    {
        if(state == (int)Menu.Game || state == (int)Menu.Pause)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                HandlePause();
            }
        }

        TimeText.text = "Time: " + gameDataInstance.prevTimer;
        MovesText.text = "Moved: " + gameDataInstance.prevMoved;
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
            paused = true;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            paused = false;
        }
    }

    public Menu GetMenuActive()
    {
        return (Menu)state;
    }

    public void StartToDifficulty()
    {
        BoardSizeSliderChange();
        SetMenuActive(Menu.Difficulty);
    }

    public async Task<bool> StartGame()
    {
        await InstanceCreator.GetPuzzleGenerator().GeneratePuzzle();
        SetMenuActive(Menu.Game);
        gameDataInstance.StartTimer();
        return await Task.FromResult(true);
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

    public void Login()
    {
        bool result = databaseInstance.userHandler.LoginUser(new User(loginUserNameInput.text,loginPasswordInput.text));
        if(result)
        {
            LoginButton.SetActive(false);
            LogoutButton.SetActive(true);
            SetMenuActive(Menu.Menu);
        }
    }

    public void Logout()
    {
        databaseInstance.userHandler.LogoutUser();
        LoginButton.SetActive(true);
        LogoutButton.SetActive(false);
    }

    public void StartToRegister()
    {
        SetMenuActive(Menu.Register);
    }
    public void Register()
    {
        bool result = databaseInstance.userHandler.RegisterUser(new User(registerUserNameInput.text, registerPasswordInput.text));
        if(result)
        {
            SetMenuActive(Menu.Menu);
        }
    }

    public void StartToLeaderboard()
    {
        SetMenuActive(Menu.Leaderboard);
    }

    public void GetLeaderBoard()
    {
        for (int i = 0; i < instantiatedRecords.Count; i++)
        {
            Destroy(instantiatedRecords[i]);
        }
        instantiatedRecords.Clear();

        List<Result> results = databaseInstance.resultHandler.GetResultsByBoardSizeAndDifficulty(difficultyDropdown.value + 1, (int)leaderboardBoardSizeSlider.value);

        for (int i = 0; i < instantiatedRecords.Count; i++)
        {
            GameObject record = Instantiate(Record);
            record.transform.parent = content;
            instantiatedRecords.Add(record);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public async void SetDifficulty(int difficulty)
    {
        gameDataInstance.difficulty = (Difficulty)difficulty;
        await StartGame();
    }

    public void HandlePause()
    {
        if(paused == false)
        {
            SetMenuActive(Menu.Pause);
        }
        else
        {
            SetMenuActive(Menu.Game);
        }
    }

    public void BoardSizeSliderChange()
    {
        gameDataInstance.boardSize = (int)boardSizeSlider.value;
        List<Result> results = databaseInstance.resultHandler.GetResultsByBoardSize(gameDataInstance.boardSize);

        for(int difficulty = 1; difficulty <= 4; difficulty++)
        {
            Result result = results.Find(res => res.difficulty == difficulty);
            if(result != null)
            {
                PRTexts[difficulty - 1].text = "Personal best\nTime: " + gameDataInstance.GetTimeInString(result.time) + "\nMoves: " + result.moved;
            }
            else
            {
                PRTexts[difficulty - 1].text = "";
            }
        }
    }
}
