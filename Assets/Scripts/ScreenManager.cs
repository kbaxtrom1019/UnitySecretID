using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public enum ScreenID
    {
       MainMenu,
       CreateGame,
       Lobby,
       OKPopup,
       Spinner 
    };


    public GameObject MainMenuScreen;
    public GameObject CreateGameScreen;
    public GameObject LobbyScreen;
    public GameObject OKPopupScreen;
    public GameObject SpinnerScreen;


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

    private void TransitionScreenOn(Animator anim)
    {
        anim.SetBool("IsDisplayed", true);
    }

    private void TransitionScreenOff(Animator anim)
    {
        anim.SetBool("IsDisplayed", false);
    }

    private GameObject GetScreenObj(ScreenID ID)
    {
        switch(ID)
        {
            case ScreenID.CreateGame:
                return CreateGameScreen;

            case ScreenID.Lobby:
                return LobbyScreen;

            case ScreenID.MainMenu:
                return MainMenuScreen;

            case ScreenID.OKPopup:
                return OKPopupScreen;

            case ScreenID.Spinner:
                return SpinnerScreen;
        }
        return null;
    }

    public CreateGameScreen GetCreateGameScreen()
    {
        GameObject Obj = GetScreenObj(ScreenID.CreateGame);
        if (Obj != null)
        {
            return Obj.GetComponent<CreateGameScreen>();
        }
        return null;
    }

    public OKPopupScreen GetOKPopupScreen()
    {
        GameObject Obj = GetScreenObj(ScreenID.CreateGame);
        if (Obj != null)
        {
            return Obj.GetComponent<OKPopupScreen>();
        }
        return null;
    }

    public void TransitionScreenOn(ScreenID ID)
    {
        GameObject Obj = GetScreenObj(ID);
        if(Obj != null)
        {
            TransitionScreenOn(Obj.GetComponent<Animator>());
        }
    }

    public void TransitionScreenOff(ScreenID ID)
    {
        GameObject Obj = GetScreenObj(ID);
        if (Obj != null)
        {
            TransitionScreenOff(Obj.GetComponent<Animator>());
        }
    }

    public void ShowOKPopup(string Msg)
    {
        GameObject Popup = GetScreenObj(ScreenID.OKPopup);
        OKPopupScreen OKPopup = Popup.GetComponent<OKPopupScreen>();
        OKPopup.SetMessageText("Please enter a name first");
        TransitionScreenOn(ScreenID.OKPopup);
    }
}
