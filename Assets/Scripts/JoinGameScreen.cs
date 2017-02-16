using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoinGameScreen : MonoBehaviour
{
    public InputField RoomKeyText;
    public InputField NameInput;

    public string GetNameText()
    {
        return NameInput.text;
    }

    public void SetNameText(string text)
    {
        NameInput.text = text;
    }

    public string GetRoomKeyText()
    {
        return RoomKeyText.text.ToUpper();
    }

}
