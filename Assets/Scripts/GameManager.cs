using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Text KeyText;
    public GameObject TextItemPrefab;
    public GameObject RoomListPanel;

    public void MainMenu_OnCreateGameButtonClicked()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        GameObject MainMenuScreen = ScreenMgr.GetMainMenuScreen();
        GameObject CreateGameScreen = ScreenMgr.GetCreateGameScreen();
        ScreenMgr.TransitionScreenOff(MainMenuScreen.GetComponent<Animator>());
        ScreenMgr.TransitionScreenOn(CreateGameScreen.GetComponent<Animator>());

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
        GameObject MainMenuScreen = ScreenMgr.GetMainMenuScreen();
        GameObject CreateGameScreen = ScreenMgr.GetCreateGameScreen();
        ScreenMgr.TransitionScreenOn(MainMenuScreen.GetComponent<Animator>());
        ScreenMgr.TransitionScreenOff(CreateGameScreen.GetComponent<Animator>());
    }

    public void CreateMenu_OnClickedCreateGame()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        GameObject CreateGameScreen = ScreenMgr.GetCreateGameScreen();
        GameObject LobbyScreen = ScreenMgr.GetLobbyScreen();
        OnlineServicesManger OnlineServices = OnlineServicesManger.GetInstance();
        if (OnlineServices.IsConnected())
        {
            CreateGameScreen CGScreen = CreateGameScreen.GetComponent<CreateGameScreen>();
            if(CGScreen.GetInputText().Length <= 0)
            {
                GameObject Popup = ScreenMgr.GetOKPopupScreen();
                OKPopupScreen OKPopup = Popup.GetComponent<OKPopupScreen>();
                OKPopup.SetMessageText("Please enter a name first");
                ScreenMgr.TransitionScreenOn(Popup.GetComponent<Animator>());
            }
            else
            {
                OnlineServices.CreateGame(OnCreateGameComplete);
                ScreenMgr.TransitionScreenOn(LobbyScreen.GetComponent<Animator>());
                ScreenMgr.TransitionScreenOff(CreateGameScreen.GetComponent<Animator>());
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
        GameObject MainMenuScreen = ScreenMgr.GetMainMenuScreen();
        GameObject LobbyMenuScreen = ScreenMgr.GetLobbyScreen();
        ScreenMgr.TransitionScreenOn(MainMenuScreen.GetComponent<Animator>());
        ScreenMgr.TransitionScreenOff(LobbyMenuScreen.GetComponent<Animator>());
    }

    public void OKPopup_OnClickedOK()
    {
        ScreenManager ScreenMgr = ScreenManager.GetInstance();
        GameObject Popup = ScreenMgr.GetOKPopupScreen();
        ScreenMgr.TransitionScreenOff(Popup.GetComponent<Animator>());
    }

    void OnAuthenticationCompleted(AuthenticationRequestResult result)
    {
        if (result.GetRequestResult() == AuthenticationRequestResult.RequestResult.Success)
        {

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