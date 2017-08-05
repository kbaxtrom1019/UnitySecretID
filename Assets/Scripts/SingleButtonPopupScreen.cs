using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SingleButtonPopupScreen : BaseMenu
{
    public Text MessageText;
    public Text ButtonText;
    private OnButtonClick onOKClicked;

    public void SetOKButtonClicked(OnButtonClick onClick)
    {
        onOKClicked = onClick;
    }

    public void SetMessageText(string text)
    {
        MessageText.text = text;
    }

    public void SetButtonText(string text)
    {
        ButtonText.text = text;
    }

    public void OnOKButtonClicked()
    {
        onOKClicked();
    }
}
