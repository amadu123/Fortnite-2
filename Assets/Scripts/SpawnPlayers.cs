using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public static float worldRadius = 80;
    public static float spawnY = 150;
    public GameObject playerPrefab;

    private void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-worldRadius, worldRadius), spawnY, Random.Range(-worldRadius, worldRadius));
        //Vector3 randomPosition = new Vector3(0, spawnY, 0);
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
