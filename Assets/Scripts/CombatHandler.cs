using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CombatHandler : MonoBehaviour
{
    public int playerHealth;
    public int playerDamage;
    public Transform rayOrigin;
    public Slider healthBar;

    PhotonView PV;
    string combatMode = "Assault Rifle";

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        if (combatMode != "None")
        {
            if (Input.GetMouseButtonDown(0))
            {
                PV.RPC("RPC_Shooting", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void RPC_Shooting()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward));
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject != gameObject && hit.transform.tag == "Player")
            {
                hit.transform.gameObject.GetComponent<CombatHandler>().TakeDamage(playerDamage);

                break;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        healthBar.value = playerHealth;
    }

    public void ChangeCombatMode(string mode)
    {
        combatMode = mode;
    }
}
