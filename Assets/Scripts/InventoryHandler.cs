using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    BuildingHandler buildingHandler;
    CombatHandler combatHandler;

    void Start()
    {
        buildingHandler = transform.Find("Main Camera").GetComponent<BuildingHandler>();
        combatHandler = GetComponent<CombatHandler>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            buildingHandler.ChangeBuildMode("Floor");
            combatHandler.ChangeCombatMode("None");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            buildingHandler.ChangeBuildMode("Wall");
            combatHandler.ChangeCombatMode("None");
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            buildingHandler.ChangeBuildMode("Stair");
            combatHandler.ChangeCombatMode("None");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            buildingHandler.ChangeBuildMode("None");
            combatHandler.ChangeCombatMode("Assault Rifle");
        }
    }
}
