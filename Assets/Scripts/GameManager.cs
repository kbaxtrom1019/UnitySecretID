using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Text KeyText;
    public GameObject TextItemPrefab;
    public GameObject RoomListPanel;
    private bool Initalized = false;


    public void Start()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        ScreenMgr.ShowSpinner("Connecting...");
    }

    public void Update()
    {
        OnlineServicesManger OnlineServices = OnlineServicesManger.GetInstance();
        if (!Initalized && OnlineServices.IsAvailable())
        {
            OnlineServices.Authenticate(OnAuthenticationCompleted);
            Initalized = true;
        }
    }

    

    public void MainMenu_OnCreateGameButtonClicked()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.MainMenu);
        ScreenMgr.TransitionScreenOn(ScreenManager.ScreenID.CreateGame);

    }

    public void MainMenu_OnJoinGameButtonClicked()
    {
        //OnlineServicesManger OnlineServices = OnlineServicesManger.GetInstance();
        //if (OnlineServices.IsConnected())
        //{

        //}
        //else
        //{
        //    // Popup message and try to connect
        //}
    }

    public void CreateMenu_OnClickedBackButton()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.CreateGame);
        ScreenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
    }

    public void CreateMenu_OnClickedCreateGame()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        OnlineServicesManger OnlineServices = OnlineServicesManger.GetInstance();
        if (OnlineServices.IsConnected())
        {
            CreateGameScreen CreateGameScreen = ScreenMgr.GetCreateGameScreen();
            if (CreateGameScreen.GetInputText().Length <= 0)
            {
                ScreenMgr.ShowOKPopup("Please enter a name first");
            }
            else
            {
                OnlineServices.CreateGame(OnCreateGameComplete);
                ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.CreateGame);
                ScreenMgr.TransitionScreenOn(ScreenManager.ScreenID.Lobby);
            }


        }
        else
        {
            // Popup message and try to connect
        }
    }

    public void LobbyMenu_OnClickedBack()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.Lobby);
        ScreenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
    }

    public void OKPopup_OnClickedOK()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.OKPopup);
    }

    void OnAuthenticationCompleted(AuthenticationRequestResult result)
    {
        if (result.GetRequestResult() == AuthenticationRequestResult.RequestResult.Success)
        {
            ScreenManager ScreenMgr = ScreenManager.GetInstance();
            ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.Spinner);
            ScreenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
        }
        else
        {

        }

    }

    void OnCreateGameComplete(CreateGameRequestResult result)
    {
        if (result.GetRequestResult() == CreateGameRequestResult.RequestResult.Success)
        {
            if(KeyText != null)
            {
                KeyText.text = result.GetData().room_key;
                GameObject Item = GameObject.Instantiate(TextItemPrefab);
                Item.GetComponent<Text>().text = result.GetData().players[0].player_id;
                Item.transform.SetParent(RoomListPanel.transform);
            }
        }
        else
        {

        }
    }
}