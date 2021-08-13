using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestructibleObjectHandler : MonoBehaviour
{
    public float health = 30;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
