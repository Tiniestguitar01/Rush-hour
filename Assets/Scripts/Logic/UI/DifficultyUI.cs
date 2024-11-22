using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DifficultyUI : MonoBehaviour
{
    UIManager manager;
    Database databaseInstance;
    GameData gameDataInstance;

    [Header("DifficultyUI")]
    public UnityEngine.UI.Slider boardSizeSlider;
    public List<TMP_Text> PRTexts;

    void Start()
    {
        manager = InstanceCreator.GetUIManager();
        databaseInstance = InstanceCreator.GetDatabase();
        gameDataInstance = InstanceCreator.GetGameData();
    }
    public async void SetDifficulty(int difficulty)
    {
        BoardSizeSliderChange();
        gameDataInstance.difficulty = (Difficulty)difficulty;
        await manager.gameUI.StartGame();
    }

    public void BoardSizeSliderChange()
    {
        gameDataInstance.boardSize = (int)boardSizeSlider.value;
        List<Result> results = databaseInstance.resultHandler.GetResultsByBoardSize(gameDataInstance.boardSize);

        for (int difficulty = 1; difficulty <= 4; difficulty++)
        {
            Result result = results.Find(res => res.difficulty == difficulty);
            if (result != null)
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
