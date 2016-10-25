using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.MainMenu);
        ScreenMgr.TransitionScreenOn(ScreenManager.ScreenID.JoinGame);
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
                OnlineServices.CreateGame(OnCreateJoinGameComplete, CreateGameScreen.GetInputText());
                ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.CreateGame);
                ScreenMgr.ShowSpinner("Creating Game...");
            }


        }
        else
        {
            // Popup message and try to connect
        }
    }

    public void JoinMenu_OnClickedJoingame()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        OnlineServicesManger OnlineServices = OnlineServicesManger.GetInstance();
        if (OnlineServices.IsConnected())
        {
            JoinGameScreen JoinGameScreen = ScreenMgr.GetJoinGameScreen();
            if (JoinGameScreen.GetNameText().Length <= 0)
            {
                ScreenMgr.ShowOKPopup("Please enter a name first");
            }
            else if (JoinGameScreen.GetRoomKeyText().Length <= 0)
            {
                ScreenMgr.ShowOKPopup("Please enter a room key first");
            }
            else
            {
                OnlineServices.JoinGame(OnCreateJoinGameComplete, JoinGameScreen.GetNameText(), JoinGameScreen.GetRoomKeyText());
                ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.JoinGame);
                ScreenMgr.ShowSpinner("Joining Game...");
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

    public void JoinMenu_OnClickedBack()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.JoinGame);
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

    void OnCreateJoinGameComplete(JoinCreateGameRequestResult result)
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        if (result.GetRequestResult() == JoinCreateGameRequestResult.RequestResult.Success)
        {
            if (KeyText != null)
            {
                JoinCreateGameRequestResult.Data data = result.GetData();
                KeyText.text = data.room_key;

                foreach (JoinCreateGameRequestResult.Player player in data.players)
                {
                    
                    GameObject Item = GameObject.Instantiate(TextItemPrefab);

                    Item.GetComponent<Text>().text = player.player_id;
                    Item.transform.SetParent(RoomListPanel.transform);

                }

                ScreenMgr.TransitionScreenOff(ScreenManager.ScreenID.Spinner);
                ScreenMgr.TransitionScreenOn(ScreenManager.ScreenID.Lobby);

            }
        }
        else
        {

        }
    }
}