using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using EazyTools.SoundManager;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Lobby,
        RefreshingLobby,
        Game,
        RefreshingGame,
        LevelComplete,
        LevelFailed
    };

    public Text KeyText;
    public GameObject TextItemPrefab;
    public GameObject RoomListPanel;
    private bool Initalized = false;
    private GameState CurrentState = GameState.None;
    private float RefreshTimer;
    private const float RefreshTime = 0.5f;
    private string LocalPlayerID = null;
    private List<Sprite> IconResources;
    private Dictionary<string, List<int>> playerIcons = new Dictionary<string, List<int>>();
    private const int MaxNumIcons = 12;
    private int CorrectButtonAward = 100;
    private int IncorrectButtonAward = -50;
    private int MaxProgress = 1000;
    private int InitialProgress = 500;
    private int Progress;
    private float ProgressLossAccumulator;
    private float ProgressLossPerSecond = 40.0f;
    private int ProgressLoss = 0;
    private int CurrentLevelIndex;
    private bool levelFinishedSent = false;
    private float MaxIconTime = 10.0f;
    private float myIconTimer;

    public AudioClip ButtonNavSnd;
    public AudioClip PopupErrorSnd;
    public AudioClip IconClickSnd;
    public AudioClip IconCompleteSnd;
    public AudioClip LevelCompleteSnd;
    public AudioClip GameOverSnd;
    public AudioClip GameMusic;
    public AudioClip LobbyMusic;
    public AudioClip FailedIconSnd;

    public void Start()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.ShowSpinner("Connecting...");
        IconLibrary library = IconLibrary.GetInstance();
        IconResources = library.GetAllIcons();
        LevelCompleteScreen levelCompleteScreen = screenMgr.GetLevelCompleteScreen();
        levelCompleteScreen.SetAnimDoneCallback(LevelCompleteMenu_AnimationComplete);
        SoundManager.PlayMusic(LobbyMusic, SoundManager.globalMusicVolume, true, true);
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

            UpdateProgress();
        }

        if(CurrentState == GameState.Game || CurrentState == GameState.RefreshingGame)
        {
            myIconTimer -= Time.deltaTime;
            float iconTimerValue = myIconTimer / MaxIconTime;
            iconTimerValue = Mathf.Clamp01(iconTimerValue);
            ScreenManager screenMgr = ScreenManager.GetInstance();
            GameScreen gameScreen = screenMgr.GetGameScreen();
            gameScreen.SetMyIconTimer(iconTimerValue);
            if (myIconTimer <= 0.0f)
            {
                SoundManager.PlayUISound(FailedIconSnd);
                PickMyIcon(false);
            }

            float meterValue = ((float)ProgressLoss + (float)Progress) / (float)MaxProgress;
            if(meterValue <= 0.25f || meterValue >= 0.8f)
            {
            }
            else if(meterValue >= 0.35f || meterValue <= 0.7f)
            {
            }
        }

    }

    int GetInitialProgressForLevel(int currLevel)
    {
        return InitialProgress;
    }

    void UpdateProgress()
    {
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        ProgressLossAccumulator += (ProgressLossPerSecond * Time.deltaTime);
        if (ProgressLossAccumulator >= 1.0f)
        {
            ProgressLoss -= 1;
            ProgressLossAccumulator = 0;
        }

        float meterValue = ((float)ProgressLoss + (float)Progress) / (float)MaxProgress;
        ScreenManager screenMgr = ScreenManager.GetInstance();
        GameScreen gameScreen = screenMgr.GetGameScreen();
        gameScreen.SetMeterProgress(meterValue);

        CheckLevelFinished(meterValue);
    }

    void CheckLevelFinished(float meterValue)
    {
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        if (levelFinishedSent == false)
        {
            if (meterValue >= 1.0f)
            {
                onlineServices.LevelFinished(true, GetInitialProgressForLevel(CurrentLevelIndex), OnLevelFinishedCompleted);
                levelFinishedSent = true;
            }

            else if (meterValue <= 0.0f)
            {
                onlineServices.LevelFinished(false, GetInitialProgressForLevel(CurrentLevelIndex), OnLevelFailedCompleted);
                levelFinishedSent = true;
            }
            
        }
   
    }

    public void OnLevelFinishedCompleted(LevelFinishedRequestResult result)
    {
        if (result.GetRequestResult() == LevelFinishedRequestResult.RequestResult.Success)
        {
            
        }
        else
        {

        }
    }

    public void OnLevelFailedCompleted(LevelFinishedRequestResult result)
    {

        if (result.GetRequestResult() == LevelFinishedRequestResult.RequestResult.Success)
        {
        }
        else
        {

        }
    }

    public void MainMenu_OnCreateGameButtonClicked()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.MainMenu);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.CreateGame);
        SoundManager.PlayUISound(ButtonNavSnd);

    }

    public void MainMenu_OnJoinGameButtonClicked()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.MainMenu);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.JoinGame);
        SoundManager.PlayUISound(ButtonNavSnd);
    }

    public void CreateMenu_OnClickedBackButton()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.CreateGame);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
        SoundManager.PlayUISound(ButtonNavSnd);
    }

    public void CreateMenu_OnNameInputEnd()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        JoinGameScreen joinScreen = screenMgr.GetJoinGameScreen();
        CreateGameScreen createScreen = screenMgr.GetCreateGameScreen();
        joinScreen.SetNameText(createScreen.GetNameText());
    }

    public void CreateMenu_OnClickedCreateGame()
    {
        
        ScreenManager screenMgr = ScreenManager.GetInstance();
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        if (onlineServices.IsConnected())
        {
            CreateGameScreen createScreen = screenMgr.GetCreateGameScreen();
            if (createScreen.GetNameText().Length <= 0)
            {
                screenMgr.ShowOKPopup("Please enter a name first");
                SoundManager.PlayUISound(PopupErrorSnd);
            }
            else
            {
                onlineServices.CreateLobby(createScreen.GetNameText(), OnCreateGameComplete);
                screenMgr.TransitionScreenOff(ScreenManager.ScreenID.CreateGame);
                screenMgr.ShowSpinner("Creating Game...");
                SoundManager.PlayUISound(ButtonNavSnd);
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
                SoundManager.PlayUISound(PopupErrorSnd);
            }
            else if (joinScreen.GetRoomKeyText().Length <= 0)
            {
                screenMgr.ShowOKPopup("Please enter a room key first");
                SoundManager.PlayUISound(PopupErrorSnd);
            }
            else
            {
                SoundManager.PlayUISound(ButtonNavSnd);
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

    public void JoinMenu_OnClickedBack()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.JoinGame);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
        SoundManager.PlayUISound(ButtonNavSnd);
    }

    public void JoinMenu_OnNameInputEnd()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        JoinGameScreen joinScreen = screenMgr.GetJoinGameScreen();
        CreateGameScreen createScreen = screenMgr.GetCreateGameScreen();
        createScreen.SetNameText(joinScreen.GetNameText());
    }


    public void LobbyMenu_OnClickedBack()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Lobby);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        onlineServices.LeaveLobby(null);
        CurrentState = GameState.None;
        SoundManager.PlayUISound(ButtonNavSnd);
    }

    public void LobbyMenu_OnClickedStart()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Lobby);
        screenMgr.ShowSpinner("Starting Game...");
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        int initProgress = GetInitialProgressForLevel(CurrentLevelIndex);
        onlineServices.StartGame(initProgress, OnStartGameCompleted);
        SoundManager.PlayUISound(ButtonNavSnd);
    }

    public void OKPopup_OnClickedOK()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.OKPopup);
        SoundManager.PlayUISound(ButtonNavSnd);
    }

    public void GameOverMenu_ClickedOK()
    {
        CurrentState = GameState.None;
        ScreenManager screenMgr = ScreenManager.GetInstance();
        screenMgr.TransitionScreenOff(ScreenManager.ScreenID.GameOver);
        screenMgr.TransitionScreenOn(ScreenManager.ScreenID.MainMenu);
        SoundManager.PlayMusic(LobbyMusic, SoundManager.globalMusicVolume, true, true);
        SoundManager.PlayUISound(ButtonNavSnd);
    }

    public void LevelCompleteMenu_AnimationComplete()
    {
        ScreenManager screenMgr = ScreenManager.GetInstance();
        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        onlineServices.RefreshLobby(OnRefreshLobbyForLevelComplete);
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
            SoundManager.PlayUISound(PopupErrorSnd);
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

    void SetupGame(int levelIndex, int seedValue, List<PlayerData> players)
    {
        CurrentLevelIndex = levelIndex;
        levelFinishedSent = false;

        int restoreSeed = Random.Range(-5000000, 5000000);
        Debug.Log("restoreSeed:" + restoreSeed);
        Debug.Log("SettingSeedValue:" + seedValue);
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

        int initProgress = GetInitialProgressForLevel(CurrentLevelIndex);
        float meterValue = (float)initProgress / (float)MaxProgress;
        gameScreen.SetMeterProgress(meterValue);


        Random.InitState(restoreSeed);
        PickMyIcon(false);
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

        SoundManager.PlayUISound(IconClickSnd);
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

    void PickMyIcon(bool iconComplete)
    {
        List<string> playerKeys = new List<string>(playerIcons.Keys);
        if (playerKeys.Count > 1)
        {
            playerKeys.Remove(LocalPlayerID);
        }

        int randPlayerIndex = Random.Range(0, playerKeys.Count);
        List<int> tempPlayerIcons = playerIcons[playerKeys[randPlayerIndex]];
        int randomIndex = Random.Range(0, tempPlayerIcons.Count);
        int iconIndex = tempPlayerIcons[randomIndex];
        Sprite img = IconResources[iconIndex];

        ScreenManager screenMgr = ScreenManager.GetInstance();
        GameScreen gameScreen = screenMgr.GetGameScreen();
        
        if(iconComplete)
        {
            gameScreen.PlayIconCompleteAnim();
            SoundManager.PlayUISound(IconCompleteSnd);
        }

        gameScreen.SetMyIcon(img);
        gameScreen.SetMyIconTimer(1.0f);

        OnlineServicesManger onlineServices = OnlineServicesManger.GetInstance();
        onlineServices.SetPlayerIcon(iconIndex, OnSetPlayerIconComplete);
        myIconTimer = MaxIconTime;
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
        item.transform.SetParent(RoomListPanel.transform, false);
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
                Progress = GetInitialProgressForLevel(CurrentLevelIndex);
                ProgressLoss = 0;
                SetupGame(data.level_index, data.seed_value, data.players);
                SoundManager.PlayMusic(GameMusic, SoundManager.globalMusicVolume, true, true);
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

    void OnRefreshLobbyForLevelComplete(RefreshLobbyRequestResult result)
    {
        if (result.GetRequestResult() == RefreshLobbyRequestResult.RequestResult.Success)
        {
            CurrentState = GameState.Game;
            RefreshLobbyRequestResult.Data data = result.GetData();
            ScreenManager screenMgr = ScreenManager.GetInstance();
            Progress = GetInitialProgressForLevel(CurrentLevelIndex);
            ProgressLoss = 0;
            SetupGame(data.level_index, data.seed_value, data.players);
            screenMgr.TransitionScreenOff(ScreenManager.ScreenID.LevelComplete);
            screenMgr.TransitionScreenOn(ScreenManager.ScreenID.Game);

        }
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
        ScreenManager screenMgr = ScreenManager.GetInstance();
        if (result.GetRequestResult() == RefreshGameRequestResult.RequestResult.Success)
        {
            RefreshGameRequestResult.Data data = result.GetData();
            if(CurrentState == GameState.Game || CurrentState == GameState.RefreshingGame)
            {
                if(data.level_index == -1)
                {
                    // Level Failed
                    CurrentState = GameState.LevelFailed;
                    screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Game);
                    screenMgr.TransitionScreenOn(ScreenManager.ScreenID.GameOver);
                    SoundManager.PlayUISound(GameOverSnd);
                }
                else if(data.level_index > CurrentLevelIndex)
                {
                    // Level complete
                    CurrentState = GameState.LevelComplete;
                    screenMgr.TransitionScreenOff(ScreenManager.ScreenID.Game);
                    screenMgr.TransitionScreenOn(ScreenManager.ScreenID.LevelComplete);
                    CurrentLevelIndex = data.level_index;
                    SoundManager.PlayUISound(LevelCompleteSnd);
                }
                else
                {
                    // Were still mid game
                    Progress = data.progress;
                    foreach (PlayerData player in data.players)
                    {
                        if (player.player_id == LocalPlayerID)
                        {
                            if (player.player_icon == -2)
                            {
                                PickMyIcon(true);
                            }
                        }
                    }
                    CurrentState = GameState.Game;
                }

            }

        }
        else
        {
            Debug.LogError("Refresh Game Error");
        }

        ResetRefreshTimer();
    }
}