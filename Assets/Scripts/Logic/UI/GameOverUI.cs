using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    UIManager manager;
    GameData gameDataInstance;

    [Header("GameOverUI")]
    public TMP_Text TimeText;
    public TMP_Text MovesText;

    void Start()
    {
        manager = InstanceCreator.GetUIManager();
        gameDataInstance = InstanceCreator.GetGameData();

    }

    void Update()
    {
        TimeText.text = "Time: " + gameDataInstance.prevTimer;
        MovesText.text = "Moved: " + gameDataInstance.prevMoved;
    }
}
