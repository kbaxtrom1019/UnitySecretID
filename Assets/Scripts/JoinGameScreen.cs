using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoinGameScreen : BaseMenu
{
    public InputField RoomKeyText;
    public InputField NameInput;

    private OnButtonClick onJoinGameClicked;
    private OnButtonClick onBackClicked;
    private OnInputEnd onNameInputEnd;
    private OnInputEnd onKeyInputEnd;

    public void SetJoinGameButtonListener(OnButtonClick onClick)
    {
        onJoinGameClicked = onClick;
    }

    public void SetBackButtonListener(OnButtonClick onClick)
    {
        onBackClicked = onClick;
    }

    public void SetNameInputEndListener(OnInputEnd onInputEnd)
    {
        onNameInputEnd = onInputEnd;
    }

    public void SetKeyInputEndListener(OnInputEnd onInputEnd)
    {
        onKeyInputEnd = onInputEnd;
    }

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

    public void OnJoinGameButtonClicked()
    {
        onJoinGameClicked();
    }

    public void OnBackButtonClicked()
    {
        onBackClicked();
    }

    public void OnNameInputEnd()
    {
        onNameInputEnd(GetNameText());
    }

    public void OnKeyInputEnd()
    {
        onKeyInputEnd(GetRoomKeyText());
    }
}
