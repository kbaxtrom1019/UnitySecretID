using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api;
using GameSparks.Core;
using GameSparks.Api.Requests;
using System;

public class PlayerData : IComparable<PlayerData>
{
    public string player_id;
    public string player_name;
    public int player_icon;

    public int CompareTo(PlayerData other)
    {
        return player_id.CompareTo(other.player_id);
    }

    //public int Compare(PlayerData x, PlayerData y)
    //{
    //    return x.player_id.CompareTo(y.player_id);
    //}
};


public abstract class BaseRequestResult
{
    public enum RequestResult
    {
        Success,
        Failure
    };

    protected RequestResult result;

    public List<PlayerData> GetPlayers(GSData resultData)
    {
        List<PlayerData> players = new List<PlayerData>();
        List<GSData> playerList = resultData.GetGSDataList("players");
        if (playerList != null)
        {
            foreach (GSData playerData in playerList)
            {
                PlayerData player = JsonUtility.FromJson<PlayerData>(playerData.JSON);
                players.Add(player);
            }
        }

        return players;
    }

    protected void Init(GSTypedResponse requestResponse)
    {
        if (requestResponse.HasErrors)
        {
            // Need to figure out what to do when a request times out
            Debug.LogError(requestResponse.Errors.JSON.ToString());
            result = RequestResult.Failure;
        }
        else
        {
            ProcessResponse(requestResponse);
            
        }
    }

    public RequestResult GetRequestResult()
    {
        return result;
    }

    protected GSData GetResultData(GSTypedResponse requestResponse)
    {
        GSData resultData = null;
        try
        {
           resultData = requestResponse.ScriptData.GetGSData("result_data");
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogException(e);
        }
        return resultData;
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
        public List<PlayerData> players = new List<PlayerData>();
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
        GSData resultData = GetResultData(requestResponse);
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
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
        public List<PlayerData> players = new List<PlayerData>();
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
        GSData resultData = GetResultData(requestResponse);
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
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
        GSData resultData = GetResultData(requestResponse);
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
        public int level_index;
        public List<PlayerData> players = new List<PlayerData>();
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
        GSData resultData = GetResultData(requestResponse);
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
        }
        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};

public class RefreshGameRequestResult : BaseRequestResult
{
    public class Data
    {
        public string error;
        public List<PlayerData> players = new List<PlayerData>();
        public int progress;
        public int level_index;
    };

    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public RefreshGameRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = GetResultData(requestResponse);
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
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
        public List<PlayerData> players = new List<PlayerData>();
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
        GSData resultData = GetResultData(requestResponse);
        data = JsonUtility.FromJson<Data>(resultData.JSON);
        data.players = GetPlayers(resultData);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
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
        GSData resultData = GetResultData(requestResponse);
        data = JsonUtility.FromJson<Data>(resultData.JSON);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
        }
        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};

public class IconPressedRequestResult : BaseRequestResult
{
    public class Data
    {
        public string error;
        public bool press_match;
    };

    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public IconPressedRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = GetResultData(requestResponse);
        data = JsonUtility.FromJson<Data>(resultData.JSON);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
        }
        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};

public class LevelFinishedRequestResult : BaseRequestResult
{
    public class Data
    {
        public string error;
    };

    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public LevelFinishedRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
    {
        Init(requestResponse);
        response = requestResponse;
    }

    protected override void ProcessResponse(GSTypedResponse requestResponse)
    {
        GSData resultData = GetResultData(requestResponse);
        data = JsonUtility.FromJson<Data>(resultData.JSON);

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }
        else
        {
            result = RequestResult.Success;
        }
        base.ProcessResponse(requestResponse);
    }

    public Data GetData()
    {
        return data;
    }
};
