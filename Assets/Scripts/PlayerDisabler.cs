using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDisabler : MonoBehaviour
{
    [SerializeField]
    GameObject Camera;
    [SerializeField]
    GameObject Canvas;

    PhotonView view;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            Camera.GetComponent<Camera>().enabled = false;
            Camera.GetComponent<AudioListener>().enabled = false;
            Canvas.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
