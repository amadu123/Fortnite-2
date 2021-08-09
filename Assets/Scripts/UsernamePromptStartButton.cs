using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UsernamePromptStartButton : MonoBehaviour
{
    public void GoToLobby()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
