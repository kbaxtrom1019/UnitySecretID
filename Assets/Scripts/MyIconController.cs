using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyIconController : MonoBehaviour
{

    public Image iconImage;
    public Image radialFill;
    private Color orange = new Color(1, 0.47f, 0.0f);

    public void SetIconTimer(float value)
    {

        if(value > 0.75f)
        {
            SetColor(radialFill, Color.green);
        }
        else if(value > 0.5f)
        {
            SetColor(radialFill, Color.yellow);
        }
        else if(value > 0.25f)
        {
            SetColor(radialFill, orange);
        }
        else
        {
            SetColor(radialFill, Color.red);
        }

        radialFill.fillAmount = value;
    }

    void setSideFillAmount(Image timerSide, float value, float cutoff)
    {
        float sideValue = (value - cutoff) / 0.25f;
        sideValue = Mathf.Clamp01(sideValue);
    }

    void ResetSide(Image timerSide)
    {
        if(timerSide.fillAmount != 1.0f)
        {
            timerSide.fillAmount = 1.0f;
        }
    }

    void SetColor(Image timerSide, Color color)
    {
        if(timerSide.color != color)
        {
            timerSide.color = color;
        }
    }
}
