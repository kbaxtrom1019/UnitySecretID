using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
    public enum ScreenID
    {
       MainMenu,
       CreateGame,
       JoinGame,
       Lobby,
       SingleButtonPopup,
       Spinner,
       Game,
       GameOver,
       LevelComplete,
       Pause,
       TwoButtonPopup,
       Max
    };

    public GameObject MainMenuScreenPrefab;
    public GameObject CreateGameScreenPrefab;
    public GameObject JoinGameScreenPrefab;
    public GameObject LobbyScreenPrefab;
    public GameObject SingleButtonPopupScreenPrefab;
    public GameObject SpinnerScreenPrefab;
    public GameObject GameScreenPrefab;
    public GameObject GameOverScreenPrefab;
    public GameObject LevelCompleteScreenPrefab;
    public GameObject PauseScreenPrefab;
    public GameObject TwoButtonPopupScreenPrefab;

    private List<GameObject> LoadedScreens;

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

        LoadedScreens = new List<GameObject>((int)ScreenID.Max);

        LoadScreen(MainMenuScreenPrefab);
        LoadScreen(CreateGameScreenPrefab);
        LoadScreen(JoinGameScreenPrefab);
        LoadScreen(LobbyScreenPrefab);
        LoadScreen(SingleButtonPopupScreenPrefab);
        LoadScreen(SpinnerScreenPrefab);
        LoadScreen(GameScreenPrefab);
        LoadScreen(GameOverScreenPrefab);
        LoadScreen(LevelCompleteScreenPrefab);
        LoadScreen(PauseScreenPrefab);
        LoadScreen(TwoButtonPopupScreenPrefab);
    }

    private void LoadScreen(GameObject prefab)
    {
        GameObject screen = GameObject.Instantiate(prefab);
        LoadedScreens.Add(screen);
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
        int idIndex = (int)ID;
        if(idIndex >= 0 && idIndex < LoadedScreens.Count)
        {
            return LoadedScreens[idIndex];
        }
        return null;
    }

    private T GetScreen<T>(ScreenID id) where T : BaseMenu
    {
        GameObject obj = GetScreenObj(id);
        if (obj != null)
        {
            return obj.GetComponent<T>();
        }
        return null;
    }

    public MainMenuScreen GetMainMenuScreen()
    {
        return GetScreen<MainMenuScreen>(ScreenID.MainMenu);
    }

    public CreateGameScreen GetCreateGameScreen()
    {
        return GetScreen<CreateGameScreen>(ScreenID.CreateGame);
    }

    public JoinGameScreen GetJoinGameScreen()
    {
        return GetScreen<JoinGameScreen>(ScreenID.JoinGame);
    }

    public GameLobbyScreen GetGameLobbyScreen()
    {
        return GetScreen<GameLobbyScreen>(ScreenID.Lobby);
    }

    public GameOverScreen GetGameOverScreen()
    {
        return GetScreen<GameOverScreen>(ScreenID.GameOver);
    }

    public GameScreen GetGameScreen()
    {
        return GetScreen<GameScreen>(ScreenID.Game);
    }

    public SingleButtonPopupScreen GetSingleButtonPopupScreen()
    {
        return GetScreen<SingleButtonPopupScreen>(ScreenID.SingleButtonPopup);
    }
    
    public TwoButtonPopupScreen GetTwoButtonPopupScreen()
    {
        return GetScreen<TwoButtonPopupScreen>(ScreenID.TwoButtonPopup);
    }

    public LevelCompleteScreen GetLevelCompleteScreen()
    {
        return GetScreen<LevelCompleteScreen>(ScreenID.LevelComplete);
    }

    public PauseScreen GetPauseScreen()
    {
        return GetScreen<PauseScreen>(ScreenID.Pause);
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

    public void ShowSingleButtonPopup(string msg, string buttonText=null)
    {
        GameObject popup = GetScreenObj(ScreenID.SingleButtonPopup);
        SingleButtonPopupScreen okPopup = popup.GetComponent<SingleButtonPopupScreen>();
        okPopup.SetMessageText(msg);
        okPopup.SetButtonText(buttonText != null ? buttonText : "OK");
        TransitionScreenOn(ScreenID.SingleButtonPopup);
    }

    public void ShowTwoButtonPopup(string msg, string leftText, string rightText, Color leftColor, Color rightColor)
    {
        GameObject popup = GetScreenObj(ScreenID.TwoButtonPopup);
        TwoButtonPopupScreen twoButtonPopup = popup.GetComponent<TwoButtonPopupScreen>();
        twoButtonPopup.SetMessageText(msg);
        twoButtonPopup.SetLeftButtonText(leftText);
        twoButtonPopup.SetRightButtonText(rightText);
        twoButtonPopup.SetLeftButtonColor(leftColor);
        twoButtonPopup.SetRightButtonColor(rightColor);
        TransitionScreenOn(ScreenID.TwoButtonPopup);

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
