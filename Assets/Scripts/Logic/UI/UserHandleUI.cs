using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserHandleUI : MonoBehaviour
{
    UIManager manager;
    Database databaseInstance;

    [Header("LoginUI")]
    public TMP_InputField loginUserNameInput;
    public TMP_InputField loginPasswordInput;

    [Header("RegisterUI")]
    public TMP_InputField registerUserNameInput;
    public TMP_InputField registerPasswordInput;

    [Header("PopUpUI")]
    public GameObject PopUp;
    public RawImage PopUpBackground;
    public TMP_Text PopUpMessageText;

    [Header("MenuUI")]
    public GameObject LoginButton;
    public GameObject RegisterButton;
    public GameObject LogoutButton;
    public TMP_Text loggedInUserText;


    void Start()
    {
        manager = InstanceCreator.GetUIManager();
        databaseInstance = InstanceCreator.GetDatabase();
    }

    public void Login()
    {
        Error result = databaseInstance.userHandler.LoginUser(new User(loginUserNameInput.text, loginPasswordInput.text));
        if (PopUp.active != true)
        {
            StartCoroutine(ShowPopUp(result));
        }
        if (result.isSuccessful)
        {
            IsLoggedIn(result.isSuccessful);
            manager.SetMenuActive(Menu.Menu);
        }
    }

    public void Logout()
    {
        databaseInstance.userHandler.LogoutUser();
        IsLoggedIn(false);
        if (PopUp.active != true)
        {
            StartCoroutine(ShowPopUp(new Error(true, "You have logged out!")));
        }
    }

    public void Register()
    {
        Error result = databaseInstance.userHandler.RegisterUser(new User(registerUserNameInput.text, registerPasswordInput.text));
        if (PopUp.active != true)
        {
            StartCoroutine(ShowPopUp(result));
        }
        if (result.isSuccessful)
        {
            IsLoggedIn(result.isSuccessful);
            manager.SetMenuActive(Menu.Menu);
        }
    }

    public void IsLoggedIn(bool logged)
    {
        if (logged)
        {
            loggedInUserText.text = "Welcome " + databaseInstance.loggedInUser.username;
        }
        else
        {
            loggedInUserText.text = "";
        }

        LoginButton.SetActive(!logged);
        RegisterButton.SetActive(!logged);
        LogoutButton.SetActive(logged);
    }

    public IEnumerator ShowPopUp(Error error)
    {
        PopUp.SetActive(true);

        if (error.isSuccessful)
        {
            PopUpBackground.color = new Color32(155, 245, 171, 255);
            PopUpMessageText.color = new Color32(155, 245, 171, 255);
        }
        else
        {
            PopUpBackground.color = new Color32(255, 132, 89, 255);
            PopUpMessageText.color = new Color32(255, 132, 89, 255);
        }

        PopUpMessageText.text = error.message;

        yield return new WaitForSeconds(PopUp.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
        if (PopUp.active == true)
        {
            PopUp.SetActive(false);
        }
    }
}
