using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Linq;

public enum Menu
{
    Menu = 0,
    Game = 1,
    Difficulty = 2,
    Pause = 3,
    Loading = 4,
    Options = 5,
    GameOver = 6,
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
    public Slider boardSizeSlider;
    public List<TMP_Text> PRTexts;

    [Header("GameOverUI")]
    public TMP_Text TimeText;
    public TMP_Text MovesText;


    GameData gameDataInstance;
    Settings settingsInstance;
    Database databaseInstance;

    private void Start()
    {
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
        databaseInstance.GetResultsByBoardSize(gameDataInstance.boardSize);

        for(int difficulty = 1; difficulty <= 4; difficulty++)
        {
            Result result = databaseInstance.results.Find(res => res.difficulty == difficulty);
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
