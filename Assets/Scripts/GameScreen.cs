using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScreen : BaseMenu
{
    public delegate void OnIconClick(int index);

    public KeypadController keypadController;
    public MyIconController myIconController;
    public MeterController progressMeterController;

    private OnButtonClick onMenuClicked;
    private OnIconClick onIconClicked;

    public void SetMenuButtonListener(OnButtonClick onClick)
    {
        onMenuClicked = onClick;
    }

    public void SetIconButtonListener(OnIconClick onClick)
    {
        onIconClicked = onClick;
    }

    public void SetKeypadIcon(int keyIndex, Sprite image)
    {
        if(keypadController != null)
        {
            keypadController.IconImages[keyIndex].sprite = image;
        }
    }

    public string GetKeypadIconName(int keyIndex)
    {
        if (keypadController != null)
        {
            return keypadController.IconImages[keyIndex].sprite.name;
        }
        return null;
    }


    public void SetMyIcon(Sprite image)
    {
        if(myIconController != null)
        {
            myIconController.iconImage.sprite = image;
        }
    }

    public void SetMyIconTimer(float value)
    {
        if (myIconController != null)
        {
            myIconController.SetIconTimer(value);
        }
    }

    public void SetMeterProgress(float value)
    {
        progressMeterController.SetToPosition(value);
    }

    public void PlayIconCompleteAnim()
    {
        GetComponent<Animator>().SetBool("IconComplete", true);
    }

    public void ClearIconCompleteFlag()
    {
        GetComponent<Animator>().SetBool("IconComplete", false);
    }

    public void OnMenuButtonClicked()
    {
        onMenuClicked();
    }

    public void OnIconButtonClicked(int index)
    {
        onIconClicked(index);
    }
}
