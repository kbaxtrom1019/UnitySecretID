using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api;
using GameSparks.Core;
using GameSparks.Api.Requests;

public class LobbyPlayer
{
    public string player_id;
    public string player_name;
};


public abstract class BaseRequestResult
{
    public enum RequestResult
    {
        Success,
        Failure
    };

    protected RequestResult result;

    public List<LobbyPlayer> GetPlayers(GSData resultData)
    {
        List<LobbyPlayer> players = new List<LobbyPlayer>();
        List<GSData> playerList = resultData.GetGSDataList("players");
        if (playerList != null)
        {
            foreach (GSData playerData in playerList)
            {
                LobbyPlayer player = JsonUtility.FromJson<LobbyPlayer>(playerData.JSON);
                players.Add(player);
            }
        }

        return players;
    }

    protected void Init(GSTypedResponse requestResponse)
    {
        if (requestResponse.HasErrors)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
        }

        ProcessResponse(requestResponse);
    }

    public RequestResult GetRequestResult()
    {
        return result;
    }

    protected virtual void ProcessResponse(GSTypedResponse requestResponse) { }
};


public class AuthenticationRequestResult : BaseRequestResult
{
    private GameSparks.Api.Responses.AuthenticationResponse response;

    public AuthenticationRequestResult(GameSparks.Api.Responses.AuthenticationResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    public string GetPlayerID()
    {
        return response.UserId;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        base.ProcessResponse(requestResponse);
    }
};

public class CreateLobbyRequestResult : BaseRequestResult
{
    public class Data
    {
        public string room_key;
        public string error;
        public List<LobbyPlayer> players = new List<LobbyPlayer>();
    };

    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public CreateLobbyRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = requestResponse.ScriptData.GetGSData("result_data");
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
    }

    public Data GetData()
    {
        return data;
    }
};

public class JoinLobbyRequestResult : BaseRequestResult
{
    public class Data
    {
        public string room_key;
        public string error;
        public List<LobbyPlayer> players = new List<LobbyPlayer>();
    };

    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public JoinLobbyRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = requestResponse.ScriptData.GetGSData("result_data");
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }

        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};

public class LeaveLobbyRequestResult : BaseRequestResult
{
    public class Data
    {
        public string error;
    };
    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public LeaveLobbyRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = requestResponse.ScriptData.GetGSData("result_data");
        data = JsonUtility.FromJson<Data>(resultData.JSON);

        if (data.error != null && data.error.Length > 0)
        {
            if (data.error == "PLAYER_NOT_IN_ROOM")
            {
                result = RequestResult.Success;
            }
            else
            {
                result = RequestResult.Failure;
            }
        }
        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};


public class RefreshLobbyRequestResult : BaseRequestResult
{
    public class Data
    {
        public string error;
        public bool game_started;
        public int seed_value;
        public List<LobbyPlayer> players = new List<LobbyPlayer>();
    };

    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public RefreshLobbyRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = requestResponse.ScriptData.GetGSData("result_data");
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};

public class StartGameRequestResult : BaseRequestResult
{

    public class Data
    {
        public string error;
        public List<LobbyPlayer> players = new List<LobbyPlayer>();
    };

    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public StartGameRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = requestResponse.ScriptData.GetGSData("result_data");
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }

        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};

public class SetPlayerIconRequestResult : BaseRequestResult
{
    public class Data
    {
        public string error;
        public List<LobbyPlayer> players = new List<LobbyPlayer>();
    };

    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public SetPlayerIconRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = requestResponse.ScriptData.GetGSData("result_data");
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};

