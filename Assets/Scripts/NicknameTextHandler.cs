using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NicknameTextHandler : MonoBehaviour
{
    public PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMesh>().text = PV.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
