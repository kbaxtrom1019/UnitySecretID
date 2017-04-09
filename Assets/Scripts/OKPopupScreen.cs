using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OKPopupScreen : BaseMenu
{
    public Text MessageText;

    public void SetMessageText(string text)
    {
        MessageText.text = text;
    }
}
