using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    Thanks = 10,
}

public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    public List<GameObject> menus;

    Menu[] navigatableMenus = { Menu.Difficulty, Menu.Leaderboard, Menu.Options, Menu.Login, Menu.Register, Menu.Thanks };

    public Menu previousMenu;
    public Menu currentMenu;

    [HideInInspector]
    public GameUI gameUI;
    [HideInInspector]
    public PauseUI pauseUI;
    [HideInInspector]
    public DifficultyUI difficultyUI;
    [HideInInspector]
    public GameOverUI gameOverUI;
    [HideInInspector]
    public UserHandleUI userHandleUI;
    [HideInInspector]
    public LeaderboardUI leaderboardUI;

    GameData gameDataInstance;

    bool inAnimation = false;


    public float animationLength = 0.5f;

    int currentInteractableIndex = 0;
    public List<Selectable> interactableElements;

    public void Start()
    {
        if(menus == null)
        {
            menus = new List<GameObject>();
        }

        gameDataInstance = InstanceCreator.GetGameData();
        gameUI = GetComponent<GameUI>();
        difficultyUI = GetComponent<DifficultyUI>();
        gameOverUI = GetComponent<GameOverUI>();
        userHandleUI = GetComponent<UserHandleUI>();
        leaderboardUI = GetComponent<LeaderboardUI>();
        pauseUI = GetComponent<PauseUI>();
        SetMenuActive(Menu.Menu);
    }

    private void Update()
    {
        switch (currentMenu)
        {
            case Menu.Login:
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    userHandleUI.Login();
                }
                break;
            }
            case Menu.Register:
            {
               if (Input.GetKeyDown(KeyCode.Return))
               {
                    userHandleUI.Register();
               }
               break;
            }
        }

        if (navigatableMenus.Contains(currentMenu) && !inAnimation)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackToPreviousMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NavigateWithTab();
        }

    }

    public void SetMenuActive(Menu menuCode)
    {
        if ((int)currentMenu >= menus.Count)
        {
            return;
        }

        previousMenu = (Menu)gameDataInstance.state;
        currentMenu = menuCode;

        StartCoroutine(waitForOutAnimation());
        for (int i = 0; i < menus[(int)currentMenu].transform.childCount; i++)
        {
            menus[(int)currentMenu].transform.GetChild(i).GetComponent<Animator>().SetBool("Out", false);
        }
 
        SetNextMenu(currentMenu);

        if (menuCode == Menu.Pause)
        {
            pauseUI.paused = true;
        }
        else
        {
            pauseUI.paused = false;
        }
    }

    public IEnumerator waitForOutAnimation()
    {
        inAnimation = true;
        if (previousMenu != Menu.Loading)
        {
            for (int i = 0; i < menus[(int)previousMenu].transform.childCount; i++)
            {
                menus[(int)previousMenu].transform.GetChild(i).GetComponent<Animator>().SetBool("Out", true);
            }
        }

        yield return new WaitForSeconds(animationLength);
        for(int i = 0; i < menus.Count; i++)
        {
            if((Menu)i != currentMenu)
            {
                menus[i].SetActive(false);
            }
        }
        SetNextMenu(currentMenu);
        inAnimation = false;
    }

    public void SetNextMenu(Menu next)
    {
        menus[(int)next].SetActive(true);
        gameDataInstance.state = (int)next;

        interactableElements.Clear();

        if (next != Menu.Loading && next != Menu.Game && next != Menu.Thanks)
        {
            foreach (Transform child in menus[(int)next].transform.Find("Interactables").transform)
            {
                interactableElements.Add(child.gameObject.GetComponent<Selectable>());
            }

            if (interactableElements.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(interactableElements[0].gameObject);
            }
        }
    }

    public void StartToDifficulty()
    {
        difficultyUI.BoardSizeSliderChange();
        SetMenuActive(Menu.Difficulty);
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

    public void StartToRegister()
    {
        SetMenuActive(Menu.Register);
    }

    public void StartToThanks()
    {
        SetMenuActive(Menu.Thanks);
    }

    public void StartToLeaderboard()
    {
        leaderboardUI.GetLeaderBoard();
        SetMenuActive(Menu.Leaderboard);
    }

    public void NavigateWithTab()
    {
        currentInteractableIndex++;

        if (currentInteractableIndex > interactableElements.Count - 1)
        {
            currentInteractableIndex = 0;
        }

        EventSystem.current.SetSelectedGameObject(interactableElements[currentInteractableIndex].gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
