using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateGameScreen : BaseMenu
{
    public InputField NameInput;
    private OnButtonClick onCreateGameClicked;
    private OnButtonClick onBackClicked;
    private OnInputEnd onNameInputEnd;

    public void SetCreateGameButtonListener(OnButtonClick onClick)
    {
        onCreateGameClicked = onClick;
    }

    public void SetBackButtonListener(OnButtonClick onClick)
    {
        onBackClicked = onClick;
    }

    public void SetNameInputEndListener(OnInputEnd onInputEnd)
    {
        onNameInputEnd = onInputEnd;
    }

    public string GetNameText()
    {
        return NameInput.text;
    }

    public void SetNameText(string text)
    {
        NameInput.text = text;
    }

    public void OnCreateGameButtonClicked()
    {
        onCreateGameClicked();
    }

    public void OnBackButtonClicked()
    {
        onBackClicked();
    }

    public void OnNameInputEnd()
    {
        onNameInputEnd(GetNameText());
    }
}
