using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuildingHandler : MonoBehaviour
{
    public float BuildDistance = 6f;
    public float GridSize = 5f;
    public float BuildWidth = 0.2f;

    public Transform Camera;
    public Transform OutlinePrefab;
    public Transform FloorPrefab;
    public Transform WallPrefab;
    public Transform StairPrefab;
    public AudioSource buildSound;

    Transform Outline;
    PhotonView PV;
    string BuildMode = "None";

    private void Start()
    {
        Outline = Instantiate(OutlinePrefab);
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!PV.IsMine) return;

        if (BuildMode != "None")
        {
            RaycastHit[] Hits = Physics.RaycastAll(Camera.position, Camera.forward, BuildDistance);
            System.Array.Sort(Hits, (x, y) => x.distance.CompareTo(y.distance));
            foreach (RaycastHit Hit in Hits)
            {
                if (Hit.transform.gameObject != gameObject)
                {
                    float BuildX = Mathf.RoundToInt(Hit.point.x / GridSize) * GridSize;
                    float BuildY = Mathf.RoundToInt(Hit.point.y / GridSize) * GridSize;
                    float BuildZ = Mathf.RoundToInt(Hit.point.z / GridSize) * GridSize;
                    Outline.position = new Vector3(BuildX, BuildY, BuildZ);

                    float BuildRotationY = Mathf.RoundToInt(Camera.eulerAngles.y / 90f) * 90f;
                    Outline.eulerAngles = new Vector3(0, BuildRotationY, 0);

                    if (Input.GetMouseButtonDown(0))
                    {
                        Build();
                    }

                    break;
                }
            }
        }
    }

    void Build()
    {
        PV.RPC("BuildSound", RpcTarget.All);

        if (BuildMode == "Floor")
        {
            PhotonNetwork.Instantiate(FloorPrefab.name, Outline.position, Outline.rotation);
        }
        else if (BuildMode == "Wall")
        {
            PhotonNetwork.Instantiate(WallPrefab.name, Outline.position, Outline.rotation);
        }
        else if (BuildMode == "Stair")
        {
            PhotonNetwork.Instantiate(StairPrefab.name, Outline.position, Outline.rotation);
        }
    }

    [PunRPC]
    void BuildSound()
    {
        buildSound.Play();
    }

    public void ChangeBuildMode(string NewBuildMode)
    {
        BuildMode = NewBuildMode;

        if (BuildMode == "Floor")
        {
            Outline.transform.GetChild(0).localPosition = FloorPrefab.transform.GetChild(0).localPosition;
            Outline.transform.GetChild(0).localRotation = FloorPrefab.transform.GetChild(0).localRotation;
            Outline.transform.GetChild(0).localScale = FloorPrefab.transform.GetChild(0).localScale;
            Outline.gameObject.SetActive(true);
        }
        else if (BuildMode == "Wall")
        {
            Outline.transform.GetChild(0).localPosition = WallPrefab.transform.GetChild(0).localPosition;
            Outline.transform.GetChild(0).localRotation = WallPrefab.transform.GetChild(0).localRotation;
            Outline.transform.GetChild(0).localScale = WallPrefab.transform.GetChild(0).localScale;
            Outline.gameObject.SetActive(true);
        }
        else if (BuildMode == "Stair")
        {
            Outline.transform.GetChild(0).localPosition = StairPrefab.transform.GetChild(0).localPosition;
            Outline.transform.GetChild(0).localRotation = StairPrefab.transform.GetChild(0).localRotation;
            Outline.transform.GetChild(0).localScale = StairPrefab.transform.GetChild(0).localScale;
            Outline.gameObject.SetActive(true);
        }
        else if (BuildMode == "None")
        {
            Outline.gameObject.SetActive(false);
        }
    }
}