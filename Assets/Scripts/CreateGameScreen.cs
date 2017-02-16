using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateGameScreen : MonoBehaviour
{
    public InputField NameInput;

    public string GetNameText()
    {
        return NameInput.text;
    }

    public void SetNameText(string text)
    {
        NameInput.text = text;
    }
}
