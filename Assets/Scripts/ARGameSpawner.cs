using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARGameSpawner : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;

    public float verticalPosOffset = 4.25f;
    public float horizontalPosOffset = 0.55f;

    private ARRaycastManager ARRaycastManager;
    private ARPlaneManager ARPlaneManager;

    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool objectInstantiated = false;
    private bool isVerticalPlacement = false;

    void Start()
    {
        ARRaycastManager = FindObjectOfType<ARRaycastManager>();
        ARPlaneManager = FindObjectOfType<ARPlaneManager>();
        placementIndicator.SetActive(true);
    }

    void Update()
    {
        if (!objectInstantiated)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        } else
        {
            if (placementIndicator.activeSelf)
            {
                placementIndicator.SetActive(false);
            }
        }
    }

    private void PlaceObject()
    {
        var spawnedObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation).gameObject;
        if (!isVerticalPlacement)
        {
            spawnedObject.transform.Translate(Vector3.up * verticalPosOffset, Space.Self);
        }
        spawnedObject.transform.Translate(Vector3.forward * horizontalPosOffset, Space.Self);
        objectInstantiated = true;
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            if (!isVerticalPlacement)
            {
                placementIndicator.transform.Translate(Vector3.up * verticalPosOffset, Space.Self);
            }
            placementIndicator.transform.Translate(Vector3.forward * horizontalPosOffset, Space.Self);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        ARRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var arPlane = ARPlaneManager.GetPlane(hits[0].trackableId);
            var planeNormal = arPlane.normal;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            if (arPlane.alignment == PlaneAlignment.Vertical)
            {
                placementPose.rotation = Quaternion.LookRotation(planeNormal, Vector3.up);
                isVerticalPlacement = true;
            } else if (arPlane.alignment == PlaneAlignment.HorizontalUp || arPlane.alignment == PlaneAlignment.HorizontalDown)
            {
                placementPose.rotation = Quaternion.LookRotation(-cameraBearing, Vector3.up);
                isVerticalPlacement = false;
            }
        }
    }
}