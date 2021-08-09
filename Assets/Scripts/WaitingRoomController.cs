using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Text playerNamesText;
    [SerializeField]
    Text playerCountText;
    [SerializeField]
    Text roomNameText;
    [SerializeField]
    Button startButton;

    int playerCount;
    string playerNames;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
        }

        PlayerCountUpdate();

        roomNameText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
    }

    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        playerCountText.text = "Players: " + playerCount;

        playerNames = "Players in room:\n";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerNames += player.NickName + "\n";
        }
        playerNamesText.text = playerNames;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel("Game");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}
