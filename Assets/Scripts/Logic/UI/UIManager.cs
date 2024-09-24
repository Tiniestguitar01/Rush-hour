using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

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
    public int state = 0; //0=menu,1=game
    public bool paused = false;

    GameData gameData;

    private void Start()
    {
        gameData = InstanceCreator.GetGameData();
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

    public async Task<bool> StartGame()
    {
        await InstanceCreator.GetPuzzleGenerator().GeneratePuzzle();
        SetMenuActive(Menu.Game);
        gameData.StartTimer();
        return await Task.FromResult(true);
    }

    public void BackToMenu()
    {
        SetMenuActive(Menu.Menu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public async void SetDifficulty(int difficulty)
    {
        gameData.difficulty = (Difficulty)difficulty;
        await StartGame();
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
