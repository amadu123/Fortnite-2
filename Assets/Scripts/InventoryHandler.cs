using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InventoryHandler : MonoBehaviour
{
    BuildingHandler buildingHandler;
    CombatHandler combatHandler;
    PhotonView PV;
    public GameObject gun;
    public GameObject blueprint;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        buildingHandler = GetComponent<BuildingHandler>();
        combatHandler = GetComponent<CombatHandler>();
        gun.SetActive(true);
        blueprint.SetActive(false);
    }

    void Update()
    {
        if (!PV.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PV.RPC("ChangeInventoryItem", RpcTarget.All, "Floor");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PV.RPC("ChangeInventoryItem", RpcTarget.All, "Wall");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PV.RPC("ChangeInventoryItem", RpcTarget.All, "Stair");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PV.RPC("ChangeInventoryItem", RpcTarget.All, "Assault Rifle");
        }
    }

    [PunRPC]
    void ChangeInventoryItem(string item)
    {
        if (item == "Floor")
        {
            buildingHandler.ChangeBuildMode("Floor");
            combatHandler.ChangeCombatMode("None");
            gun.SetActive(false);
            blueprint.SetActive(true);
        }
        else if (item == "Wall")
        {
            buildingHandler.ChangeBuildMode("Wall");
            combatHandler.ChangeCombatMode("None");
            gun.SetActive(false);
            blueprint.SetActive(true);
        }
        else if (item == "Stair")
        {
            buildingHandler.ChangeBuildMode("Stair");
            combatHandler.ChangeCombatMode("None");
            gun.SetActive(false);
            blueprint.SetActive(true);
        }
        else if (item == "Assault Rifle")
        {
            buildingHandler.ChangeBuildMode("None");
            combatHandler.ChangeCombatMode("Assault Rifle");
            gun.SetActive(true);
            blueprint.SetActive(false);
        }
    }
}
