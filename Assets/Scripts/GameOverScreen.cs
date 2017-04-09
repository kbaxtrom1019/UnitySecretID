using UnityEngine;
using System.Collections;

public class GameOverScreen : BaseMenu
{
    private OnButtonClick onOKClicked;

    public void SetOKButtonListener(OnButtonClick onClick)
    {
        onOKClicked = onClick;
    }

    public void OnOKButtonClicked()
    {
        onOKClicked();
    }
}
