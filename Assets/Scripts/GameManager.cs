using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Text KeyText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCreateGameButtonClicked()
    {
        OnlineServicesManger OnlineServices = OnlineServicesManger.GetInstance();
        if (OnlineServices.IsConnected())
        {
            OnlineServices.CreateGame(OnCreateGameComplete);
        }
        else
        {
            // Popup message and try to connect
        }

    }

    public void OnJoinGameButtonClicked()
    {
        OnlineServicesManger OnlineServices = OnlineServicesManger.GetInstance();
        if (OnlineServices.IsConnected())
        {

        }
        else
        {
            // Popup message and try to connect
        }
    }

    void OnAuthenticationCompleted(AuthenticationRequestResult result)
    {
        if (result.GetRequestResult() == AuthenticationRequestResult.RequestResult.Success)
        {

        }
        else
        {

        }

    }

    void OnCreateGameComplete(CreateGameRequestResult result)
    {
        if (result.GetRequestResult() == CreateGameRequestResult.RequestResult.Success)
        {
            if(KeyText != null)
            {
                KeyText.text = result.GetData().room_key;
            }
        }
        else
        {

        }
    }
}