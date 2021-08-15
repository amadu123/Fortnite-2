using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ManagePlayers : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Hashtable setProps = new Hashtable();
        setProps.Add("playerCount", (int)PhotonNetwork.CurrentRoom.PlayerCount);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setProps, null, null);
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int prevPlayerCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["playerCount"];

        Hashtable setProps = new Hashtable();
        setProps.Add("playerCount", prevPlayerCount - 1);

        Hashtable expectedProps = new Hashtable();
        expectedProps.Add("playerCount", prevPlayerCount);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setProps, expectedProps, null);
    }
}
