using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    UIManager manager;

    public bool paused = false;

    void Start()
    {
        manager = InstanceCreator.GetUIManager();
    }

    void Update()
    {
        if (manager.state == (int)Menu.Game || manager.state == (int)Menu.Pause)
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
        }
        else
        {
            manager.SetMenuActive(Menu.Game);
        }
    }
}
