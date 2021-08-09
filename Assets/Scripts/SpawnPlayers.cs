using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float worldRadius;
    public float spawnY;

    private void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-worldRadius, worldRadius), spawnY, Random.Range(-worldRadius, worldRadius));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
