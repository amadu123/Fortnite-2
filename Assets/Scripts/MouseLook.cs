using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed = 100f;
    public Transform playerBody;
    public GameObject pausePanel;
    float xRotation = 0f;
    PhotonView PV;
    bool pausingDisabled = false;

    void Start()
    {
        PV = transform.parent.GetComponent<PhotonView>();
        if (!PV.IsMine) return;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!PV.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Escape) && !pausingDisabled)
        {
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

    }

    public void Unpause()
    {
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisablePausing()
    {
        pausingDisabled = true;
    }
}
