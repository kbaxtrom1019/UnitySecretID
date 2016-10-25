using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api;
using GameSparks.Core;
using GameSparks.Api.Requests;

public class AuthenticationRequestResult 
{
    public enum RequestResult
    {
        Success,
        Failure
    };

    public RequestResult result;
    private GameSparks.Api.Responses.AuthenticationResponse response;

    public AuthenticationRequestResult(GameSparks.Api.Responses.AuthenticationResponse requestResponse)
    {
        if (requestResponse.HasErrors)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
        }

        response = requestResponse;
    }

    public RequestResult GetRequestResult()
    {
        return result;
    }
};



public class JoinCreateGameRequestResult
{
    public enum RequestResult
    {
        Success,
        Failure
    };

    public class Player
    {
        public string player_id;
    };

    public class Data
    {
        public string room_key;
        public string errors;
        public List<Player> players = new List<Player>();
    };

    public RequestResult result;
    private string roomKey;
    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public JoinCreateGameRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        if (requestResponse.HasErrors)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
        }

        GSData resultData = requestResponse.ScriptData.GetGSData("result_data");
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        List<GSData> playerList = resultData.GetGSDataList("players");
        if(playerList != null)
        {
            data.players = new List<Player>();
            foreach (GSData playerData in playerList)
            {
                Player player = JsonUtility.FromJson<Player>(playerData.JSON);
                data.players.Add(player);
            }
        }

        response = requestResponse;
    }

    public RequestResult GetRequestResult()
    {
        return result;
    }

    public string GetRoomKey()
    {
        return roomKey;
    }

    public Data GetData()
    {
        return data;
    }
};


public class OnlineServicesManger : MonoBehaviour
{
    public delegate void OnAuthenticationComplete(AuthenticationRequestResult result);
    public delegate void OnCreateGameComplete(JoinCreateGameRequestResult result);
    public delegate void OnJoinGameComplete(JoinCreateGameRequestResult result);

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

    public void CreateGame(OnCreateGameComplete callback, string playerName)
    {
        GSRequestData data = new GSRequestData();
        data.AddString("player_name", playerName);
        new LogEventRequest()
        .SetEventKey("CREATE_GAME")
        .SetEventAttribute("cg_data", data)
        .Send((response) => {

            if(callback != null)
            {
                JoinCreateGameRequestResult result = new JoinCreateGameRequestResult(response);
                callback(result);
            }
    });
    }

    public void JoinGame(OnJoinGameComplete callback, string playerName, string roomKey)
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
                JoinCreateGameRequestResult result = new JoinCreateGameRequestResult(response);
                callback(result);
            }
        });
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
