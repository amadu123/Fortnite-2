using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(createInput.text))
        {
            PhotonNetwork.CreateRoom(createInput.text);
        }
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(joinInput.text))
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Waiting");
    }
}
