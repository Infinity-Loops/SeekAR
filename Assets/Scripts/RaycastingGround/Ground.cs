using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class Ground : MonoBehaviour
{
    public Transform ground;
    private ARRaycastManager raycaster;
    private Vector2 screenCenter;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycaster = GetComponent<ARRaycastManager>();
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    private void Update()
    {
        if (raycaster == null) return;

        hits.Clear();

        if (raycaster.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon) && hits.Count > 0)
        {
            Vector3 hitPose = hits[0].pose.position;
            ground.position = hitPose;
        }
    }
}
