using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api;
using GameSparks.Core;
using GameSparks.Api.Requests;


public class OnlineServicesManger : MonoBehaviour
{
    public delegate void OnAuthenticationComplete(AuthenticationRequestResult result);
    public delegate void OnCreateLobbyComplete(CreateLobbyRequestResult result);
    public delegate void OnJoinLobbyComplete(JoinLobbyRequestResult result);
    public delegate void OnLeaveLobbyComplete(LeaveLobbyRequestResult result);
    public delegate void OnRefreshLobbyComplete(RefreshLobbyRequestResult result);
    public delegate void OnStartGameComplete(StartGameRequestResult result);
    public delegate void OnSetPlayerIconComplete(SetPlayerIconRequestResult result);

    private static OnlineServicesManger instance;
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
    }

    public static OnlineServicesManger GetInstance()
    {
        return instance;
    }

    private const int itemWidth = 200;
    private const int itemHeight = 30;

    public bool IsConnected()
    {
        return IsAvailable() && IsAuthenticated();
    }

    public bool IsAvailable()
    {
        return GS.Available;
    }

    public bool IsAuthenticated()
    {
        return GS.Authenticated;
    }

    public void Authenticate(OnAuthenticationComplete callback)
    {
        new DeviceAuthenticationRequest()
            .SetDisplayName("Valis")
            .Send((response) => {

            if(callback != null)
            {
                AuthenticationRequestResult result = new AuthenticationRequestResult(response);
                callback(result);
            }
            string msg = string.Format("Authentication Response\nUserID:{0}\nJson:{1}\nErrors:{2}", response.UserId, response.JSONString, response.Errors);
            Debug.Log(msg);
        });
    }

    public void CreateLobby(OnCreateLobbyComplete callback, string playerName)
    {
        GSRequestData leavedata = new GSRequestData();
        new LogEventRequest()
        .SetEventKey("LEAVE_GAME")
        .SetEventAttribute("lg_data", leavedata)
        .Send((leaveResponse) => {

            LeaveLobbyRequestResult leaveResult = new LeaveLobbyRequestResult(leaveResponse);
            if(leaveResult.GetRequestResult() == LeaveLobbyRequestResult.RequestResult.Success)
            {
                GSRequestData data = new GSRequestData();
                data.AddString("player_name", playerName);
                new LogEventRequest()
                .SetEventKey("CREATE_GAME")
                .SetEventAttribute("cg_data", data)
                .Send((createResponse) => {

                    if (callback != null)
                    {
                        CreateLobbyRequestResult createResult = new CreateLobbyRequestResult(createResponse);
                        callback(createResult);
                    }
                });
            }
            else
            {
                if (callback != null)
                {
                    CreateLobbyRequestResult createResult = new CreateLobbyRequestResult(leaveResponse);
                    callback(createResult);
                }
            }
        });


    }

    public void JoinLobby(OnJoinLobbyComplete callback, string playerName, string roomKey)
    {
        GSRequestData data = new GSRequestData();
        data.AddString("player_name", playerName);
        data.AddString("room_key", roomKey);
        new LogEventRequest()
        .SetEventKey("JOIN_GAME")
        .SetEventAttribute("jg_data", data)
        .Send((response) => {

            if (callback != null)
            {
                JoinLobbyRequestResult result = new JoinLobbyRequestResult(response);
                callback(result);
            }
        });
    }

    public void LeaveLobby(OnLeaveLobbyComplete callback)
    {
        GSRequestData data = new GSRequestData();
        new LogEventRequest()
        .SetEventKey("LEAVE_GAME")
        .SetEventAttribute("lg_data", data)
        .Send((response) => {

            if (callback != null)
            {
                LeaveLobbyRequestResult result = new LeaveLobbyRequestResult(response);
                callback(result);
            }
        });
    }

    public void RefreshLobby(OnRefreshLobbyComplete callback)
    {
        GSRequestData data = new GSRequestData();
        new LogEventRequest()
        .SetEventKey("REFRESH_GAME")
        .SetEventAttribute("rg_data", data)
        .Send((response) => {

            if (callback != null)
            {
                RefreshLobbyRequestResult result = new RefreshLobbyRequestResult(response);
                callback(result);
            }
        });
    }

    public void StartGame(OnStartGameComplete callback)
    {
        int seedValue = Random.Range(0, 10000000);
        GSRequestData data = new GSRequestData();
        data.AddNumber("seed_value", seedValue);

        new LogEventRequest()
        .SetEventKey("START_GAME")
        .SetEventAttribute("sg_data", data)
        .Send((response) => {

            if (callback != null)
            {
                StartGameRequestResult result = new StartGameRequestResult(response);
                callback(result);
            }
        });
    }

    public void SetPlayerIcon(int iconIndex, OnSetPlayerIconComplete callback)
    {

    }

    public void IconButtonPressed(int iconIndex)
    {

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label((GS.Available ? "AVAILABLE" : "NOT AVAILABLE"), GUILayout.Width(itemWidth), GUILayout.Height(itemHeight));
        GUILayout.Label(("SDK Version: " + GS.Version.ToString()), GUILayout.Width(itemWidth), GUILayout.Height(itemHeight));

        GUILayout.EndHorizontal();

        GUILayout.Label((GS.Authenticated ? "AUTHENTICATED" : "NOT AUTHENTICATED"), GUILayout.Width(itemWidth), GUILayout.Height(itemHeight));

    }
}
