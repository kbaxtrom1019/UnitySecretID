using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Lobby,
        RefreshingLobby,
        Game,
        RefreshingGame,
    };

    public Text KeyText;
    public GameObject TextItemPrefab;
    public GameObject RoomListPanel;
    private bool Initalized = false;
    private GameState CurrentState = GameState.None;
    private float RefreshTimer;
    private const float RefreshTime = 2.0f;
    private string LocalPlayerID = null;
    private List<Sprite> IconResources;
    private Dictionary<string, List<int>> playerIcons = new Dictionary<string, List<int>>();
    private const int MaxNumIcons = 9;

    public void Start()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.ShowSpinner("Connecting...");
        IconResources = new List<Sprite>(Resources.LoadAll<Sprite>("GameIcons"));
    }

    public void Update()
    {
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        if (!Initalized && onlineServices.IsAvailable())
        {
            onlineServices.Authenticate(OnAuthenticationCompleted);
            Initalized = true;
        }

        if(CurrentState == GameState.Lobby)
        {
            RefreshTimer -= Time.deltaTime;
            if(RefreshTimer <= 0.0f)
            {
                CurrentState = GameState.RefreshingLobby;
                onlineServices.RefreshLobby(OnRefreshLobbyComplete);
            }
        }
    }

    public void MainMenu_OnCreateGameButtonClicked()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.MainMenu);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.CreateGame);

    }

    public void MainMenu_OnJoinGameButtonClicked()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.MainMenu);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.JoinGame);
    }

    public void CreateMenu_OnClickedBackButton()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.CreateGame);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
    }

    public void CreateMenu_OnClickedCreateGame()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        if (onlineServices.IsConnected())
        {
            CreateGameScreen createScreen = screenMgr.GetCreateGameScreen();
            if (createScreen.GetInputText().Length <= 0)
            {
                screenMgr.ShowOKPopup("Please enter a name first");
            }
            else
            {
                onlineServices.CreateLobby(OnCreateGameComplete, createScreen.GetInputText());
                screenMgr.TransitionScreenOff(ScreenManager.ScreenID.CreateGame);
                screenMgr.ShowSpinner("Creating Game...");
            }
        }
        else
        {
            // Popup message and try to connect
        }
    }

    public void JoinMenu_OnClickedJoingame()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        if (onlineServices.IsConnected())
        {
            JoinGameScreen joinScreen = screenMgr.GetJoinGameScreen();
            if (joinScreen.GetNameText().Length <= 0)
            {
                screenMgr.ShowOKPopup("Please enter a name first");
            }
            else if (joinScreen.GetRoomKeyText().Length <= 0)
            {
                screenMgr.ShowOKPopup("Please enter a room key first");
            }
            else
            {
                onlineServices.JoinLobby(OnJoinGameComplete, joinScreen.GetNameText(), joinScreen.GetRoomKeyText());
                screenMgr.TransitionScreenOff(ScreenManager.ScreenID.JoinGame);
                screenMgr.ShowSpinner("Joining Game...");
            }

        }
        else
        {
            // Popup message and try to connect
        }
    }


    public void LobbyMenu_OnClickedBack()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Lobby);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        onlineServices.LeaveLobby(null);
        CurrentState = GameState.None;
    }

    public void LobbyMenu_OnClickedStart()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Lobby);
        screenMgr.ShowSpinner("Starting Game...");
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        onlineServices.StartGame(OnStartGameCompleted);

    }

    public void JoinMenu_OnClickedBack()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.JoinGame);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
    }

    public void OKPopup_OnClickedOK()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.OKPopup);
    }

    void OnAuthenticationCompleted(AuthenticationRequestResult result)
    {
        if (result.GetRequestResult() == AuthenticationRequestResult.RequestResult.Success)
        {
            ScreenManager screenMgr = ScreenManager.GetInstance();
            screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Spinner);
            screenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
            LocalPlayerID = result.GetPlayerID();
        }
        else
        {
            Debug.LogError("Authention Error");
        }

    }

    void OnCreateGameComplete(CreateLobbyRequestResult result)
    {
        if (result.GetRequestResult() == CreateLobbyRequestResult.RequestResult.Success)
        {
            CreateLobbyRequestResult.Data data = result.GetData();
            SetupLobby(data.room_key, data.players);

        }
        else
        {
            ScreenManager screenMgr = ScreenManager.GetInstance();
            screenMgr.ShowOKPopup("An error occured while creating a game.  Please try again");
            screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Spinner);
            screenMgr.TransitionScreenOn(ScreenManager.ScreenID.CreateGame);
        }
    }

    void OnJoinGameComplete(JoinLobbyRequestResult result)
    {
        if (result.GetRequestResult() == JoinLobbyRequestResult.RequestResult.Success)
        {
            JoinLobbyRequestResult.Data data = result.GetData();
            SetupLobby(data.room_key, data.players);
        }
        else
        {
            ScreenManager screenMgr = ScreenManager.GetInstance();
            screenMgr.ShowOKPopup("An error occured while creating joining the game.  Please try again");
            screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Spinner);
            screenMgr.TransitionScreenOn(ScreenManager.ScreenID.JoinGame);
        }
    }

    void OnStartGameCompleted(StartGameRequestResult result)
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        if (result.GetRequestResult() == StartGameRequestResult.RequestResult.Success)
        {
            
        }
        else
        {
            screenMgr.ShowOKPopup("Error starting game.  Please try again");
        }
    }

    void SetupGame(int seedValue, List<LobbyPlayer> players)
    {
        Random.InitState(seedValue);
        players.Sort();


        int numIconsNeeded = players.Count * MaxNumIcons;
        int totalNumIcons = IconResources.Count;
        List<int> AllIconIndicies = new List<int>();
        for(int i = 0; i < totalNumIcons; ++i)
        {
            AllIconIndicies.Add(i);
        }

        playerIcons.Clear();
        foreach (LobbyPlayer player in players)
        {
            List<int> tempPlayerIcons = new List<int>();
            for(int j = 0; j < MaxNumIcons; ++j)
            {
                int randIndex = Random.Range(0, AllIconIndicies.Count - 1);
                int index = AllIconIndicies[randIndex];
                AllIconIndicies.RemoveAt(randIndex);
                tempPlayerIcons.Add(index);
            }

            playerIcons.Add(player.player_id, tempPlayerIcons);
            string playerDataStr = string.Format("Player:{0} ID:{1}", player.player_name, player.player_id);
            Debug.Log(playerDataStr);
            if (player.player_id == LocalPlayerID)
            {
                ScreenManager screenMgr = ScreenManager.GetInstance();
                GameScreen gameScreen = screenMgr.GetGameScreen();
                string iconList = "Icons:";
                for (int j = 0; j < MaxNumIcons; ++j)
                {
                    Sprite img = IconResources[tempPlayerIcons[j]];
                    gameScreen.SetKeypadIcon(j, img);
                    iconList += string.Format("{0} ", img.name);

                }
                Debug.Log(iconList);
            }
            
        }


        PickMyIcon();
    }

    public void GameMenu_OnIconButtonPressed(int buttonIndex)
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        GameScreen gameScreen = screenMgr.GetGameScreen();
        string iconName = gameScreen.GetKeypadIconName(buttonIndex);

        List<int> buttonMapping = playerIcons[LocalPlayerID];
        int pressedIndex = buttonMapping[buttonIndex];

        Sprite icon = IconResources[pressedIndex];
        Debug.Assert(icon.name == iconName, "Icon name does not match:"+iconName);

    }

    void PickMyIcon()
    {
        List<string> playerKeys = new List<string>(playerIcons.Keys);
        int randPlayerIndex = Random.Range(0, playerKeys.Count - 1);
        List<int> tempPlayerIcons = playerIcons[playerKeys[randPlayerIndex]];
        int randomIndex = Random.Range(0, tempPlayerIcons.Count - 1);
        int iconIndex = tempPlayerIcons[randomIndex];
        Sprite img = IconResources[iconIndex];

        ScreenManager screenMgr = ScreenManager.GetInstance();
        GameScreen gameScreen = screenMgr.GetGameScreen();
        gameScreen.SetMyIcon(img);

        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        onlineServices.SetPlayerIcon(iconIndex, OnSetPlayerIconComplete);
    }

    void OnSetPlayerIconComplete(SetPlayerIconRequestResult result)
    {
        if(result.GetRequestResult() == SetPlayerIconRequestResult.RequestResult.Success)
        {

        }
        else
        {
            Debug.LogError("Set player icon failed");
        }
    }

    void ResetRefreshTimer()
    {
        RefreshTimer = RefreshTime;
    }

    void AddPlayerToLobby(LobbyPlayer player)
    {
        GameObject item = GameObject.Instantiate(TextItemPrefab);
        LobbyOccupant occupant = item.GetComponent<LobbyOccupant>();
        occupant.SetPlayerNameText(player.player_name, player.player_id);
        item.transform.SetParent(RoomListPanel.transform);
    }

    void SetupLobby(string roomKey, List<LobbyPlayer> players)
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        KeyText.text = roomKey;

        for(int i = 0; i < RoomListPanel.transform.childCount; ++i)
        {
            GameObject.Destroy(RoomListPanel.transform.GetChild(i).gameObject);
        }

        foreach (LobbyPlayer player in players)
        {
            AddPlayerToLobby(player);
        }

        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Spinner);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.Lobby);
        CurrentState = GameState.Lobby;
        ResetRefreshTimer();
    }

    void OnRefreshLobbyComplete(RefreshLobbyRequestResult result)
    {
        if (result.GetRequestResult() == RefreshLobbyRequestResult.RequestResult.Success)
        {
            RefreshLobbyRequestResult.Data data = result.GetData();
            if (data.game_started && CurrentState == GameState.RefreshingLobby)
            {
                CurrentState = GameState.Game;

                ScreenManager screenMgr = ScreenManager.GetInstance();
                screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Lobby);
                if (screenMgr.IsShowing(ScreenManager.ScreenID.Spinner))
                {
                    screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Spinner);
                }
                SetupGame(data.seed_value, data.players);
                screenMgr.TransitionScreenOn(ScreenManager.ScreenID.Game);
            }
            else
            {
                UpdateLobby(data.players);
                CurrentState = GameState.Lobby;
            }

        }
        else
        {
            Debug.LogError("Refresh Game Error");
        }
        ResetRefreshTimer();
    }

    void UpdateLobby(List<LobbyPlayer> players)
    {
        bool localPlayerInLobby = false;

        foreach (LobbyPlayer player in players)
        {
            if(player.player_id == LocalPlayerID)
            {
                localPlayerInLobby = true;
            }
        }

        if (localPlayerInLobby)
        {
            // Remove any lost players
            int numChildren = RoomListPanel.transform.childCount;
            for (int i = numChildren - 1; i >= 0; --i)
            {
                GameObject obj = RoomListPanel.transform.GetChild(i).gameObject;
                LobbyOccupant occupant = obj.GetComponent<LobbyOccupant>();
                string playerID = occupant.GetPlayerID();
                bool playerIsInList = false;
                foreach (LobbyPlayer player in players)
                {
                    if (player.player_id == playerID)
                    {
                        playerIsInList = true;
                    }
                }

                if (playerIsInList == false)
                {
                    GameObject.Destroy(obj);
                }
            }

            numChildren = RoomListPanel.transform.childCount;
            // Add any new players
            foreach (LobbyPlayer player in players)
            {
                bool playerAlreadyExists = false;
                for (int i = 0; i < numChildren && !playerAlreadyExists; ++i)
                {
                    GameObject obj = RoomListPanel.transform.GetChild(i).gameObject;
                    LobbyOccupant occupant = obj.GetComponent<LobbyOccupant>();
                    if(occupant.GetPlayerID() == player.player_id)
                    {
                        playerAlreadyExists = true;
                    }
                }

                if(playerAlreadyExists == false)
                {
                    AddPlayerToLobby(player);
                }
            }
        }
        else
        {
            // We have been removed from the lobby
        }
    }
}