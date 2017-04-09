using UnityEngine;
using System.Collections;

public class GameLobbyScreen : BaseMenu
{
    private OnButtonClick onStartGameClicked;
    private OnButtonClick onBackClicked;


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
}
