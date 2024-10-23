using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    UIManager manager;
    Database databaseInstance;
    GameData gameDataInstance;

    [Header("LeaderboardUI")]
    public UnityEngine.UI.Slider leaderboardBoardSizeSlider;
    public TMP_Dropdown difficultyDropdown;
    public Transform content;
    public GameObject Record;
    List<GameObject> instantiatedRecords;

    void Start()
    {
        instantiatedRecords = new List<GameObject>();
        manager = InstanceCreator.GetUIManager();
        databaseInstance = InstanceCreator.GetDatabase();
        gameDataInstance = InstanceCreator.GetGameData();
    }

    public void GetLeaderBoard()
    {
        for (int i = 0; i < instantiatedRecords.Count; i++)
        {
            Destroy(instantiatedRecords[i]);
        }
        instantiatedRecords.Clear();

        List<UserResultDTO> results = databaseInstance.resultHandler.GetResultsByBoardSizeAndDifficulty(difficultyDropdown.value + 1, (int)leaderboardBoardSizeSlider.value);

        for (int i = 0; i < results.Count; i++)
        {
            GameObject record = Instantiate(Record);
            record.transform.parent = content;
            record.GetComponent<TMP_Text>().text = results[i].username + ": " + "Moved: " + results[i].moved + ", Time:" + gameDataInstance.GetTimeInString(results[i].time);
            instantiatedRecords.Add(record);
        }

        if(results.Count == 0)
        {
            GameObject record = Instantiate(Record);
            record.transform.parent = content;
            record.GetComponent<TMP_Text>().text = "No records found in this difficulty and board size!";
            instantiatedRecords.Add(record);
        }
    }
}
