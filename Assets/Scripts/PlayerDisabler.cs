using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDisabler : MonoBehaviour
{
    PhotonView view;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            GetComponent<CharacterController>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            transform.Find("Main Camera").gameObject.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
