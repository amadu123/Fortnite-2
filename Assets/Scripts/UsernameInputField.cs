using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UsernameInputField : MonoBehaviour
{
    void Start()
    {
        string defaultUsername = string.Empty;
        InputField usernameInput = GetComponent<InputField>();
        if (PlayerPrefs.HasKey("Username"))
        {
            defaultUsername = PlayerPrefs.GetString("Username");
            usernameInput.text = defaultUsername;
        }
        PhotonNetwork.NickName = defaultUsername;
    }

    public void SetUsername(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;


        PlayerPrefs.SetString("Username", value);
    }
}
