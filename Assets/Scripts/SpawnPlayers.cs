using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public static float worldRadius = 80;
    public static float spawnY = 150;
    public GameObject playerPrefab;

    private void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-worldRadius, worldRadius), spawnY, Random.Range(-worldRadius, worldRadius));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
