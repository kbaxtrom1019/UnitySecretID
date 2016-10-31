using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyOccupant : MonoBehaviour
{
    public Text PlayerNameText;
    private string PlayerID;
	// Use this for initialization
    public void SetPlayerNameText(string text, string playerId)
    {
        PlayerNameText.text = text;
        PlayerID = playerId;
    }

    public string GetPlayerID()
    {
        return PlayerID;
    }
}
