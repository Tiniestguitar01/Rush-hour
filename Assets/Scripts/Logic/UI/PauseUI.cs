using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    UIManager manager;
    GameData gameData;

    public bool paused = false;

    void Start()
    {
        manager = InstanceCreator.GetUIManager();
        gameData = InstanceCreator.GetGameData();
    }

    void Update()
    {
        if (gameData.state == (int)Menu.Game || gameData.state == (int)Menu.Pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandlePause();
            }
        }
    }

    public void HandlePause()
    {
        if (paused == false)
        {
            manager.SetMenuActive(Menu.Pause);
            gameData.StopTimer();
        }
        else
        {
            manager.SetMenuActive(Menu.Game);
            gameData.StartTimer(false);
        }
    }
}
