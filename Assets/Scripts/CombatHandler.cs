using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CombatHandler : MonoBehaviourPunCallbacks
{
    public int lives = 5;
    public int maxHealth = 100;
    public int playerHealth = 100;
    public int playerDamage = 10;
    public Transform rayOrigin;
    public Slider healthBar;
    public Text playerCountText;
    public Text soloText;
    public Text placeText;
    public MouseLook mouseLook;
    public PlayerMovement playerMovement;
    public GameObject capsule;
    public GameObject gun;
    public GameObject blueprint;
    public CharacterController characterController;
    public Camera cam;
    public ParticleSystem muzzleFlash;
    public Camera deathCam;
    public GameObject victoryPanel;
    public GameObject alivePanel;
    public AudioSource gunshotSound;

    PhotonView PV;
    string combatMode = "Assault Rifle";
    bool soloMode = false;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            soloMode = true;
            soloText.gameObject.SetActive(true);
        }

        PV = GetComponent<PhotonView>();
        playerHealth = maxHealth;
        ChangePlayerProperty("alive", true);
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        if (combatMode != "None")
        {
            if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked)
            { 
                Shoot();
            }
        }

        object playerCount = PhotonNetwork.CurrentRoom.CustomProperties["playerCount"];
        if (playerCount != null)
        {
            playerCountText.text = "Players left: " + ((int)playerCount).ToString();

            if ((int)playerCount == 1 && !soloMode)
            {
                VicRoy();
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
        PV.RPC("PlayGunShot", RpcTarget.All);

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
    void PlayGunShot()
    {
        muzzleFlash.Play();
        gunshotSound.Play();
    }

    [PunRPC]
    void RemoveBuild(int target)
    {
        PhotonView.Find(target).gameObject.SetActive(false);
    }

    [PunRPC]
    void RemovePlayer()
    {
        Destroy(gameObject);
    }

    void Dead()
    {
        cam.GetComponent<AudioListener>().enabled = false;
        Instantiate(deathCam, cam.transform.position, cam.transform.rotation);
        ChangePlayerProperty("alive", false);
        DecreasePlayerCount();
        mouseLook.Unpause();
        mouseLook.DisablePausing();
        Cursor.lockState = CursorLockMode.None;

        PV.RPC("RemovePlayer", RpcTarget.All);
    }

    public void VicRoy()
    {
        alivePanel.SetActive(false);
        victoryPanel.SetActive(true);
        mouseLook.Unpause();
        mouseLook.DisablePausing();
        Cursor.lockState = CursorLockMode.None;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!PV.IsMine) return;

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

    void DecreasePlayerCount()
    {
        int prevPlayerCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["playerCount"];

        Hashtable setProps = new Hashtable();
        setProps.Add("playerCount", prevPlayerCount - 1);

        Hashtable expectedProps = new Hashtable();
        expectedProps.Add("playerCount", prevPlayerCount);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setProps, expectedProps, null);
    }

    void ChangePlayerProperty(string key, bool value)
    {
        Hashtable setProps = new Hashtable();
        setProps.Add(key, value);
        PhotonNetwork.LocalPlayer.SetCustomProperties(setProps, null, null);
    }
}
