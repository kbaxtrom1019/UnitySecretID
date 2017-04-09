using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpinnerScreen : BaseMenu
{
    public Text MessageText;

    public void SetMessageText(string text)
    {
        MessageText.text = text;
    }
}
