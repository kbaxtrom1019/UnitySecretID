using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoinGameScreen : MonoBehaviour
{
    public Text NameText;
    public Text RoomKeyText;

    public string GetNameText()
    {
        return NameText.text;
    }

    public string GetRoomKeyText()
    {
        return RoomKeyText.text;
    }

}
