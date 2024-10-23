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
    Solver solverInstance;

    [Header("GameUI")]
    public TMP_Text TimerText;
    public TMP_Text MovedText;
    public TMP_Text SolvableText;
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
        solverInstance = InstanceCreator.GetSolver();
        tutorialPopUp.SetActive(false);
        if (!tutorialShown)
        {
            tutorialPopUp.SetActive(true);
        }
        tutorialPopUpAnimator = tutorialPopUp.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            tutorialShown = true;
            TutorialOff();
        }

        TutorialOff();
    }

    public async Task<bool> StartGame()
    {
        manager.SetMenuActive(Menu.Loading);
        await InstanceCreator.GetPuzzleGenerator().GeneratePuzzle();
        manager.SetMenuActive(Menu.Game);
        gameDataInstance.StartTimer();
        SolvableText.text = "Solvable in " + solverInstance.stepsToSolve + " steps";
        return await Task.FromResult(true);
    }

    public void TutorialOff()
    {
        if (gameDataInstance.moved > 0 || tutorialShown == true )
        {
            tutorialPopUpAnimator.SetBool("finished", true);
            tutorialShown = true;
            tutorialPopUp.SetActive(false);
        }
    }
}
