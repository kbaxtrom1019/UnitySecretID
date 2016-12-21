using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public enum ScreenID
    {
       MainMenu,
       CreateGame,
       JoinGame,
       Lobby,
       OKPopup,
       Spinner,
       Game,
       GameOver,
       LevelComplete
    };


    public GameObject MainMenuScreen;
    public GameObject CreateGameScreen;
    public GameObject JoinGameScreen;
    public GameObject LobbyScreen;
    public GameObject OKPopupScreen;
    public GameObject SpinnerScreen;
    public GameObject GameScreen;
    public GameObject GameOverScreen;
    public GameObject LevelCompleteScreen;

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

        MainMenuScreen.SetActive(true);
        CreateGameScreen.SetActive(true);
        JoinGameScreen.SetActive(true);
        LobbyScreen.SetActive(true);
        OKPopupScreen.SetActive(true);
        SpinnerScreen.SetActive(true);
        GameScreen.SetActive(true);
        GameOverScreen.SetActive(true);
        LevelCompleteScreen.SetActive(true);
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

            case ScreenID.JoinGame:
                return JoinGameScreen;

            case ScreenID.Lobby:
                return LobbyScreen;

            case ScreenID.MainMenu:
                return MainMenuScreen;

            case ScreenID.OKPopup:
                return OKPopupScreen;

            case ScreenID.Spinner:
                return SpinnerScreen;

            case ScreenID.Game:
                return GameScreen;

            case ScreenID.GameOver:
                return GameOverScreen;

            case ScreenID.LevelComplete:
                return LevelCompleteScreen;
        }
        return null;
    }

    public CreateGameScreen GetCreateGameScreen()
    {
        GameObject obj = GetScreenObj(ScreenID.CreateGame);
        if (obj != null)
        {
            return obj.GetComponent<CreateGameScreen>();
        }
        return null;
    }

    public JoinGameScreen GetJoinGameScreen()
    {
        GameObject obj = GetScreenObj(ScreenID.JoinGame);
        if (obj != null)
        {
            return obj.GetComponent<JoinGameScreen>();
        }
        return null;
    }

    public GameScreen GetGameScreen()
    {
        GameObject obj = GetScreenObj(ScreenID.Game);
        if (obj != null)
        {
            return obj.GetComponent<GameScreen>();
        }
        return null;
    }

    public OKPopupScreen GetOKPopupScreen()
    {
        GameObject obj = GetScreenObj(ScreenID.CreateGame);
        if (obj != null)
        {
            return obj.GetComponent<OKPopupScreen>();
        }
        return null;
    }
    
    public LevelCompleteScreen GetLevelCompleteScreen()
    {
        GameObject obj = GetScreenObj(ScreenID.LevelComplete);
        if (obj != null)
        {
            return obj.GetComponent<LevelCompleteScreen>();
        }
        return null;
    }

    public void TransitionScreenOn(ScreenID id)
    {
        GameObject obj = GetScreenObj(id);
        if(obj != null)
        {
            TransitionScreenOn(obj.GetComponent<Animator>());
        }
    }

    public void TransitionScreenOff(ScreenID id)
    {
        GameObject obj = GetScreenObj(id);
        if (obj != null)
        {
            TransitionScreenOff(obj.GetComponent<Animator>());
        }
    }

    public void ShowOKPopup(string Msg)
    {
        GameObject popup = GetScreenObj(ScreenID.OKPopup);
        OKPopupScreen okPopup = popup.GetComponent<OKPopupScreen>();
        okPopup.SetMessageText(Msg);
        TransitionScreenOn(ScreenID.OKPopup);
    }

    public void ShowSpinner(string Msg)
    {
        GameObject spinnerObj = GetScreenObj(ScreenID.Spinner);
        SpinnerScreen spinner = spinnerObj.GetComponent<SpinnerScreen>();
        spinner.SetMessageText(Msg);
        TransitionScreenOn(ScreenID.Spinner);
    }

    public bool IsShowing(ScreenID id)
    {
        bool retVal = false;
        GameObject obj = GetScreenObj(id);
        if (obj != null)
        {
            Animator anim = obj.GetComponent<Animator>();
            retVal = anim.GetBool("IsDisplayed");
        }
        return retVal;
    }
}
