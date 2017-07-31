using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameLobbyScreen : BaseMenu
{
    private OnButtonClick onStartGameClicked;
    private OnButtonClick onBackClicked;
    public GameObject occupantPrefab;
    public GameObject roomListPanel;
    public Text KeyText;

    public void SetStartGameButtonListener(OnButtonClick onClick)
    {
        onStartGameClicked = onClick;
    }

    public void SetBackButtonListener(OnButtonClick onClick)
    {
        onBackClicked = onClick;
    }

    public void OnBackButtonClicked()
    {
        onBackClicked();
    }

    public void OnStartButtonClicked()
    {
        onStartGameClicked();
    }

    public void AddPlayer(PlayerData player)
    {
        GameObject item = GameObject.Instantiate(occupantPrefab);
        LobbyOccupant occupant = item.GetComponent<LobbyOccupant>();
        occupant.SetPlayerNameText(player.player_name, player.player_id);
        item.transform.SetParent(roomListPanel.transform, false);
    }

    public void DestoryAllOccupants()
    {
        for (int i = 0; i < roomListPanel.transform.childCount; ++i)
        {
            GameObject.Destroy(roomListPanel.transform.GetChild(i).gameObject);
        }
    }

    public void UpdatePlayerList(List<PlayerData> players)
    {
        // Remove any lost players
        int numChildren = roomListPanel.transform.childCount;
        for (int i = numChildren - 1; i >= 0; --i)
        {
            GameObject obj = roomListPanel.transform.GetChild(i).gameObject;
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

        numChildren = roomListPanel.transform.childCount;
        // Add any new players
        foreach (PlayerData player in players)
        {
            bool playerAlreadyExists = false;
            for (int i = 0; i < numChildren && !playerAlreadyExists; ++i)
            {
                GameObject obj = roomListPanel.transform.GetChild(i).gameObject;
                LobbyOccupant occupant = obj.GetComponent<LobbyOccupant>();
                if (occupant.GetPlayerID() == player.player_id)
                {
                    playerAlreadyExists = true;
                }
            }

            if (playerAlreadyExists == false)
            {
                AddPlayer(player);
            }
        }

    }

    public void SetKeyText(string text)
    {
        KeyText.text = text;
    }
}
