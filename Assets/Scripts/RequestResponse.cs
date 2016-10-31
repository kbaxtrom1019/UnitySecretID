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

    public string GetPlayerID()
    {
        return response.UserId;
    }
};

public class LobbyPlayer
{
    public string player_id;
    public string player_name;
};

public class CreateLobbyRequestResult
{
    public enum RequestResult
    {
        Success,
        Failure
    };

    public class Data
    {
        public string room_key;
        public string error;
        public List<LobbyPlayer> players = new List<LobbyPlayer>();
    };

    public RequestResult result;
    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public CreateLobbyRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
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
        if (playerList != null)
        {
            data.players = new List<LobbyPlayer>();
            foreach (GSData playerData in playerList)
            {
                LobbyPlayer player = JsonUtility.FromJson<LobbyPlayer>(playerData.JSON);
                data.players.Add(player);
            }
        }

        if (data.error != null)
        {
            result = RequestResult.Failure;
        }

        response = requestResponse;
    }

    public RequestResult GetRequestResult()
    {
        return result;
    }

    public Data GetData()
    {
        return data;
    }
};

public class JoinLobbyRequestResult
{
    public enum RequestResult
    {
        Success,
        Failure
    };

    public class Data
    {
        public string room_key;
        public string error;
        public List<LobbyPlayer> players = new List<LobbyPlayer>();
    };

    public RequestResult result;
    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public JoinLobbyRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
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
        if (playerList != null)
        {
            data.players = new List<LobbyPlayer>();
            foreach (GSData playerData in playerList)
            {
                LobbyPlayer player = JsonUtility.FromJson<LobbyPlayer>(playerData.JSON);
                data.players.Add(player);
            }
        }

        if(data.error != null)
        {
            result = RequestResult.Failure;
        }

        response = requestResponse;
    }

    public RequestResult GetRequestResult()
    {
        return result;
    }

    public Data GetData()
    {
        return data;
    }
};

public class LeaveLobbyRequestResult
{
    public enum RequestResult
    {
        Success,
        Failure
    };

    public class Data
    {
        public string error;
    };

    public RequestResult result;
    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public LeaveLobbyRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
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

        if(data.error != null && data.error.Length > 0)
        {
            if(data.error == "PLAYER_NOT_IN_ROOM")
            {
                result = RequestResult.Success;
            }
            else
            {
                result = RequestResult.Failure;
            }
        }

        response = requestResponse;
    }

    public RequestResult GetRequestResult()
    {
        return result;
    }

    public Data GetData()
    {
        return data;
    }
};


public class RefreshLobbyRequestResult
{
    public enum RequestResult
    {
        Success,
        Failure
    };

    public class Data
    {
        public string error;
        public List<LobbyPlayer> players = new List<LobbyPlayer>();
    };

    public RequestResult result;
    private GameSparks.Api.Responses.LogEventResponse response;
    private Data data;

    public RefreshLobbyRequestResult(GameSparks.Api.Responses.LogEventResponse requestResponse)
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
        if (playerList != null)
        {
            data.players = new List<LobbyPlayer>();
            foreach (GSData playerData in playerList)
            {
                LobbyPlayer player = JsonUtility.FromJson<LobbyPlayer>(playerData.JSON);
                data.players.Add(player);
            }
        }

        if (data.error != null && data.error.Length > 0)
        {
            result = RequestResult.Failure;
        }

        response = requestResponse;
    }

    public RequestResult GetRequestResult()
    {
        return result;
    }

    public Data GetData()
    {
        return data;
    }
};