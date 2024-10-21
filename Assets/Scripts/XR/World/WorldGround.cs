using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class WorldGround : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public Transform camera;
    public Transform ground;
    private bool isDetectingSurface;


    void HandleSystem()
    {
        Vector2 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        List<ARRaycastHit> surfaceHits = new List<ARRaycastHit>();
        
        isDetectingSurface = raycastManager.Raycast(screenCenter, surfaceHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);

        if (isDetectingSurface)
        {
            Vector3 groundLocation = surfaceHits[0].pose.position;
            ground.transform.position = new Vector3(camera.position.x, groundLocation.y, camera.position.z);
        }
    }
    private void Update()
    {
        HandleSystem();
    }
}
