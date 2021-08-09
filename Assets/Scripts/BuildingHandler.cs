using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuildingHandler : MonoBehaviour
{
    public float BuildDistance = 6f;
    public float GridSize = 5f;
    public float BuildWidth = 0.2f;

    public Transform FloorOutline;
    public Transform WallOutline;
    public Transform StairOutline;

    public Transform FloorPrefab;
    public Transform WallPrefab;
    public Transform StairPrefab;

    RaycastHit Hit;
    Transform Outline;
    string BuildMode = "None";

    private void Start()
    {
        Outline = Instantiate(FloorOutline);
    }

    void Update()
    {
        if (BuildMode != "None")
        {
            if (Physics.Raycast(transform.position, transform.forward, out Hit, BuildDistance, ~LayerMask.GetMask(LayerMask.LayerToName(transform.parent.gameObject.layer))))
            {
                Debug.DrawLine(transform.position, Hit.point, Color.green);

                Outline.gameObject.SetActive(true);

                float BuildX = Mathf.RoundToInt(Hit.point.x / GridSize) * GridSize;
                float BuildY = Mathf.RoundToInt(Hit.point.y / GridSize) * GridSize;
                float BuildZ = Mathf.RoundToInt(Hit.point.z / GridSize) * GridSize;
                Outline.position = new Vector3(BuildX, BuildY, BuildZ);

                float BuildRotationY = Mathf.RoundToInt(transform.eulerAngles.y / 90f) * 90f;
                Outline.eulerAngles = new Vector3(0, BuildRotationY, 0);

            }
            else
            {
                Outline.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Build();
            }
        }
    }

    void Build()
    {
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

    public void ChangeBuildMode(string NewBuildMode)
    {
        BuildMode = NewBuildMode;

        if (BuildMode == "Floor")
        {
            Outline.transform.GetChild(0).localPosition = FloorOutline.transform.GetChild(0).localPosition;
            Outline.transform.GetChild(0).localRotation = FloorOutline.transform.GetChild(0).localRotation;
            Outline.transform.GetChild(0).localScale = FloorOutline.transform.GetChild(0).localScale;
            Outline.gameObject.SetActive(true);
        }
        else if (BuildMode == "Wall")
        {
            Outline.transform.GetChild(0).localPosition = WallOutline.transform.GetChild(0).localPosition;
            Outline.transform.GetChild(0).localRotation = WallOutline.transform.GetChild(0).localRotation;
            Outline.transform.GetChild(0).localScale = WallOutline.transform.GetChild(0).localScale;
            Outline.gameObject.SetActive(true);
        }
        else if (BuildMode == "Stair")
        {
            Outline.transform.GetChild(0).localPosition = StairOutline.transform.GetChild(0).localPosition;
            Outline.transform.GetChild(0).localRotation = StairOutline.transform.GetChild(0).localRotation;
            Outline.transform.GetChild(0).localScale = StairOutline.transform.GetChild(0).localScale;
            Outline.gameObject.SetActive(true);
        }
        else if (BuildMode == "None")
        {
            Outline.gameObject.SetActive(false);
        }
    }
}