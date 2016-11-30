using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    public KeypadController keypadController;
    public MyIconController myIconController;
    public MeterController progressMeterController;

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

    public void SetMeterProgress(float value)
    {
        progressMeterController.SetToPosition(value);
    }

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
