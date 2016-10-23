using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateGameScreen : MonoBehaviour
{
    public Text InputText;

    public string GetInputText()
    {
        return InputText.text;
    }
}
