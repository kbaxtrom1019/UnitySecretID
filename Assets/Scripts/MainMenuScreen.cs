using UnityEngine;
using System.Collections;

public class MainMenuScreen : BaseMenu
{
    private OnButtonClick onCreateGameClicked;
    private OnButtonClick onJoinGameClicked;

    public void SetCreateGameButtonListener(OnButtonClick onClick)
    {
        onCreateGameClicked = onClick;
    }

    public void SetJoinGameButtonListener(OnButtonClick onClick)
    {
        onJoinGameClicked = onClick;
    }

    public void OnCreateGameButtonClicked()
    {
        onCreateGameClicked();
    }

    public void OnJoinGameButtonClicked()
    {
        onJoinGameClicked();
    }
}
