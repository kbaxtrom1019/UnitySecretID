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
    private int CorrectButtonAward = 100;
    private int IncorrectButtonAward = -50;
    private int MaxProgress = 1000;
    private int InitialProgress = 500;
    private int Progress;
    private float ProgressLossAccumulator;
    private float ProgressLossPerSecond = 16.0f;
    private int ProgressLoss = 0;

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

        if (CurrentState == GameState.Lobby)
        {
            RefreshTimer -= Time.deltaTime;
            if (RefreshTimer <= 0.0f)
            {
                CurrentState = GameState.RefreshingLobby;
                onlineServices.RefreshLobby(OnRefreshLobbyComplete);
            }
        }

        if (CurrentState == GameState.Game)
        {
            RefreshTimer -= Time.deltaTime;
            if (RefreshTimer <= 0.0f)
            {
                CurrentState = GameState.RefreshingGame;
                onlineServices.RefreshGame(OnRefreshGameComplete);
            }

            ProgressLossAccumulator += (ProgressLossPerSecond * Time.deltaTime);
            if(ProgressLossAccumulator >= 1.0f)
            {
                ProgressLoss -= 1;
                ProgressLossAccumulator = 0;
            }

            float meterValue = ((float)ProgressLoss + (float)Progress) / (float)MaxProgress;
            ScreenManager screenMgr = ScreenManager.GetInstance();
            GameScreen gameScreen = screenMgr.GetGameScreen();
            gameScreen.SetMeterProgress(meterValue);
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
                onlineServices.CreateLobby(createScreen.GetInputText(), OnCreateGameComplete);
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
                onlineServices.JoinLobby(joinScreen.GetNameText(), joinScreen.GetRoomKeyText(), OnJoinGameComplete);
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
        onlineServices.StartGame(InitialProgress, OnStartGameCompleted);
        Progress = InitialProgress;
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

    void SetupGame(int seedValue, List<PlayerData> players)
    {
        int restoreSeed = Random.Range(-5000000, 5000000);
        Debug.Log("restoreSeed:" + restoreSeed);
        Random.InitState(seedValue);
        players.Sort();

        int totalNumIcons = IconResources.Count;
        List<int> AllIconIndicies = new List<int>();
        for (int i = 0; i < totalNumIcons; ++i)
        {
            AllIconIndicies.Add(i);
        }
        ScreenManager screenMgr = ScreenManager.GetInstance();
        GameScreen gameScreen = screenMgr.GetGameScreen();
        playerIcons.Clear();
        foreach (PlayerData player in players)
        {
            List<int> tempPlayerIcons = new List<int>();
            for (int j = 0; j < MaxNumIcons; ++j)
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

        float meterValue = (float)InitialProgress / (float)MaxProgress;
        gameScreen.SetMeterProgress(meterValue);


        Random.InitState(restoreSeed);
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
        Debug.Assert(icon.name == iconName, "Icon name does not match:" + iconName);

        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        onlineServices.IconButtonPressed(pressedIndex, CorrectButtonAward, IncorrectButtonAward, OnIconButtonPressedComplete);
    }

    void OnIconButtonPressedComplete(IconPressedRequestResult result)
    {
        if (result.GetRequestResult() == IconPressedRequestResult.RequestResult.Success)
        {
            IconPressedRequestResult.Data data = result.GetData();
            if (data.press_match)
            {

            }
            else
            {

            }
        }
        else
        {
            Debug.LogError("Set player icon failed");
        }

    }

    void PickMyIcon()
    {
        List<string> playerKeys = new List<string>(playerIcons.Keys);
        if (playerKeys.Count > 1)
        {
            playerKeys.Remove(LocalPlayerID);
        }

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
        if (result.GetRequestResult() == SetPlayerIconRequestResult.RequestResult.Success)
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

    void AddPlayerToLobby(PlayerData player)
    {
        GameObject item = GameObject.Instantiate(TextItemPrefab);
        LobbyOccupant occupant = item.GetComponent<LobbyOccupant>();
        occupant.SetPlayerNameText(player.player_name, player.player_id);
        item.transform.SetParent(RoomListPanel.transform);
    }

    void SetupLobby(string roomKey, List<PlayerData> players)
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        KeyText.text = roomKey;

        for (int i = 0; i < RoomListPanel.transform.childCount; ++i)
        {
            GameObject.Destroy(RoomListPanel.transform.GetChild(i).gameObject);
        }

        foreach (PlayerData player in players)
        {
            AddPlayerToLobby(player);
        }
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        onlineServices.SetRoomKey(roomKey);
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

    void UpdateLobby(List<PlayerData> players)
    {
        bool localPlayerInLobby = false;

        foreach (PlayerData player in players)
        {
            if (player.player_id == LocalPlayerID)
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
                foreach (PlayerData player in players)
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
            foreach (PlayerData player in players)
            {
                bool playerAlreadyExists = false;
                for (int i = 0; i < numChildren && !playerAlreadyExists; ++i)
                {
                    GameObject obj = RoomListPanel.transform.GetChild(i).gameObject;
                    LobbyOccupant occupant = obj.GetComponent<LobbyOccupant>();
                    if (occupant.GetPlayerID() == player.player_id)
                    {
                        playerAlreadyExists = true;
                    }
                }

                if (playerAlreadyExists == false)
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

    void OnRefreshGameComplete(RefreshGameRequestResult result)
    {

        if (result.GetRequestResult() == RefreshGameRequestResult.RequestResult.Success)
        {
            RefreshGameRequestResult.Data data = result.GetData();
            if(CurrentState == GameState.Game || CurrentState == GameState.RefreshingGame)
            {
                Progress = data.progress;
                foreach (PlayerData player in data.players)
                {
                    if (player.player_id == LocalPlayerID)
                    {
                        if (player.player_icon == -2)
                        {
                            PickMyIcon();
                        }
                    }
                }
            }

        }
        else
        {
            Debug.LogError("Refresh Game Error");
        }

        CurrentState = GameState.Game;
        ResetRefreshTimer();
    }
}