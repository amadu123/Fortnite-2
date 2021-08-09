using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CombatHandler : MonoBehaviour
{
    public int playerHealth;
    public int playerDamage;
    public Transform rayOrigin;

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
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer))))
        {
            if (hit.transform.tag == "Player")
            {
                hit.transform.gameObject.GetComponent<CombatHandler>().TakeDamage(playerDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
    }

    public void ChangeCombatMode(string mode)
    {
        combatMode = mode;
    }
}
