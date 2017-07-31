using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TwoButtonPopupScreen : BaseMenu
{
    public Text MessageText;
    public Text LeftText;
    public Text RightText;
    public Image LeftButton;
    public Image RightButton;
    private OnButtonClick onLeftClicked;
    private OnButtonClick onRightClicked;

    public void SetLeftButtonClicked(OnButtonClick onClick)
    {
        onLeftClicked = onClick;
    }

    public void SetRightButtonClicked(OnButtonClick onClick)
    {
        onRightClicked = onClick;
    }

    public void SetMessageText(string text)
    {
        MessageText.text = text;
    }

    public void SetLeftButtonText(string text)
    {
        LeftText.text = text;
    }

    public void SetLeftButtonColor(Color color)
    {
        LeftButton.color = color;
    }

    public void SetRightButtonColor(Color color)
    {
        RightButton.color = color;
    }

    public void SetRightButtonText(string text)
    {
        RightText.text = text;
    }

    public void OnLeftButtonClicked()
    {
        onLeftClicked();
    }

    public void OnRightButtonClicked()
    {
        onRightClicked();
    }
}
