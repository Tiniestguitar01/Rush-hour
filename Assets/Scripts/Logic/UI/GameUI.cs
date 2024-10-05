using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    UIManager manager;
    GameData gameDataInstance;

    [Header("GameUI")]
    public TMP_Text TimerText;
    public TMP_Text MovedText;
    public int state = 0; //0=menu,1=game
    public bool paused = false;
    public Menu previousMenu;
    public GameObject tutorialPopUp;
    Animator tutorialPopUpAnimator;
    bool tutorialShown = false;
    bool movedCamera = false;
    bool scrolledCamera = false;


    void Start()
    {
        manager = InstanceCreator.GetUIManager();
        gameDataInstance = InstanceCreator.GetGameData();

        if (!tutorialShown)
        {
            tutorialPopUp.SetActive(true);
        }
        tutorialPopUpAnimator = tutorialPopUp.GetComponent<Animator>();
    }

    private void Update()
    {
        TutorialOff();
    }

    public async Task<bool> StartGame()
    {
        await InstanceCreator.GetPuzzleGenerator().GeneratePuzzle();
        manager.SetMenuActive(Menu.Game);
        gameDataInstance.StartTimer();
        return await Task.FromResult(true);
    }

    public void TutorialOff()
    {

        if (gameDataInstance.moved > 0)
        {
            tutorialPopUpAnimator.SetBool("finished", true);
            tutorialShown = true;
        }
    }
}
