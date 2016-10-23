using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public GameObject MainMenuScreen;
    public GameObject CreateGameScreen;
    public GameObject LobbyScreen;
    public GameObject OKPopupScreen;

    public GameObject GetMainMenuScreen()
    {
        return MainMenuScreen;
    }

    public GameObject GetCreateGameScreen()
    {
        return CreateGameScreen;
    }

    public GameObject GetLobbyScreen()
    {
        return LobbyScreen;
    }

    public GameObject GetOKPopupScreen()
    {
        return OKPopupScreen;
    }

    private static ScreenManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        TransitionScreenOn(MainMenuScreen.GetComponent<Animator>());
    }

    public static ScreenManager GetInstance()
    {
        return instance;
    }

    public void TransitionScreenOn(Animator anim)
    {
        anim.SetBool("IsDisplayed", true);
    }

    public void TransitionScreenOff(Animator anim)
    {
        anim.SetBool("IsDisplayed", false);
    }
}
