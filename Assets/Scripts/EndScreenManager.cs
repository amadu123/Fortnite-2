using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public Text placeText;

    void Start()
    {
        if (placeText != null)
        {
            placeText.text = "You placed: " + ((int)PhotonNetwork.CurrentRoom.CustomProperties["playerCount"]).ToString() + "th";
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}