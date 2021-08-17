using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed = 100f;
    public Transform playerBody;
    public GameObject pausePanel;
    public GameObject livePanel;
    float xRotation = 0f;
    PhotonView PV;
    bool pausingDisabled = false;
    bool paused = false;

    void Start()
    {
        PV = transform.parent.GetComponent<PhotonView>();
        if (!PV.IsMine) return;
    }

    void Update()
    {
        if (!PV.IsMine) return;

        if (Input.GetMouseButtonDown(0) && !paused && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.O) && !pausingDisabled)
        {
            Pause();
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

    public void Pause()
    {
        paused = true;
        pausePanel.SetActive(true);
        livePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        paused = false;
        pausePanel.SetActive(false);
        livePanel.SetActive(true);
    }

    public void DisablePausing()
    {
        pausingDisabled = true;
    }
}
