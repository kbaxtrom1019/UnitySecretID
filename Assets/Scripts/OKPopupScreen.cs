using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OKPopupScreen : BaseMenu
{
    public Text MessageText;
    private OnButtonClick onOKClicked;

    public void SetOKButtonClicked(OnButtonClick onClick)
    {
        onOKClicked = onClick;
    }

    public void SetMessageText(string text)
    {
        MessageText.text = text;
    }

    public void OnOKButtonClicked()
    {
        onOKClicked();
    }
}
