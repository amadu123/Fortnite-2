using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CombatHandler : MonoBehaviour
{
    public int lives = 5;
    public int maxHealth = 100;
    public int playerHealth = 100;
    public int playerDamage = 10;
    public int playerCount = 0;
    public Transform rayOrigin;
    public Slider healthBar;
    public Text playerCountText;
    public Text soloText;
    public GameObject alivePanel;
    public GameObject deadPanel;
    public GameObject victoryRoyale;
    public Text placeText;
    public MouseLook mouseLook;
    public PlayerMovement playerMovement;
    public GameObject capsule;
    public CharacterController characterController;
    public Camera cam;
    public ParticleSystem muzzleFlash;

    PhotonView PV;
    string combatMode = "Assault Rifle";
    bool soloMode = false;
    bool dead = false;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            soloMode = true;
            soloText.gameObject.SetActive(true);
        }

        PV = GetComponent<PhotonView>();
        playerHealth = maxHealth;
        UpdateOthersPlayerCount(1, true);
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        if (combatMode != "None")
        {
            if (Input.GetMouseButtonDown(0) && !dead)
            { 
                Shoot();
            }
        }
    }
    public void ChangeCombatMode(string mode)
    {
        combatMode = mode;
    }

    void Spawn()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-SpawnPlayers.worldRadius, SpawnPlayers.worldRadius), SpawnPlayers.spawnY, Random.Range(-SpawnPlayers.worldRadius, SpawnPlayers.worldRadius));
        transform.position = randomPosition;
    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit[] hits;
        hits = Physics.RaycastAll(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward));
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject != gameObject)
            {
                if (hit.transform.tag == "Player")
                {
                    hit.transform.gameObject.GetComponent<CombatHandler>().PV.RPC("TakeDamage", RpcTarget.All, playerDamage);
                    break;
                }
                else if (hit.transform.tag == "Destructible")
                {
                    hit.transform.gameObject.GetComponent<DestructibleObjectHandler>().TakeDamage(playerDamage);

                    if (hit.transform.gameObject.GetComponent<DestructibleObjectHandler>().health <= 0)
                    {
                        PV.RPC("RemoveBuild", RpcTarget.All, hit.transform.parent.gameObject.GetPhotonView().ViewID);
                    }

                    break;
                }
            }
        }
    }

    [PunRPC]
    void RemoveBuild(int target)
    {
        PhotonView.Find(target).gameObject.SetActive(false);
    }

    void Dead()
    {
        dead = true;
        alivePanel.SetActive(false);
        deadPanel.SetActive(true);
        victoryRoyale.SetActive(false);
        placeText.gameObject.SetActive(true);

        placeText.text = "You placed: " + playerCount + "th";

        UpdateOthersPlayerCount(-1, false);

        capsule.SetActive(false);
        characterController.enabled = false;
        mouseLook.GameOver();
        playerMovement.GameOver();
        mouseLook.Unpause();
    }

    void VicRoy()
    {
        dead = true;
        alivePanel.SetActive(false);
        deadPanel.SetActive(true);
        victoryRoyale.SetActive(true);
        placeText.gameObject.SetActive(false);
        mouseLook.GameOver();
        playerMovement.GameOver();
        mouseLook.Unpause();
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        healthBar.value = playerHealth;

        if (playerHealth <= 0)
        {
            playerHealth = maxHealth;
            lives--;
            if (lives <= 0)
            {
                Dead();
            }
            else
            {
                Spawn();
            }
        }

    }

    [PunRPC]
    void UpdateOthersPlayerCount(int playersChanged, bool start)
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in photonViews)
        {
            var player = view.Owner;

            if (player != null)
            {
                CombatHandler combatHandler = view.gameObject.GetComponent<CombatHandler>();
                if (combatHandler != null) combatHandler.UpdatePlayerCount(playersChanged, start);
            }
        }
    }

    public void UpdatePlayerCount(int playersChanged, bool start)
    {
        playerCount += playersChanged;
        playerCountText.text = "Players left: " + playerCount;
        if(playerCount == 1 && !soloMode && !start && !dead)
        {
            VicRoy();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
