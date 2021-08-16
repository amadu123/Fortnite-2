using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(createInput.text))
        {
            Photon.Realtime.RoomOptions setProps = new Photon.Realtime.RoomOptions();
            setProps.CleanupCacheOnLeave = false;

            PhotonNetwork.CreateRoom(createInput.text, setProps);
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
