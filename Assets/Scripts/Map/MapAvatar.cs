using Niantic.Lightship.Maps;
using Niantic.Lightship.Maps.Core.Coordinates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class MapAvatar : MonoBehaviour
{
    public static MapAvatar Instance;
    public Transform avatarRoot;
    public Transform mapCamera;
    public LightshipMapView map;
    public Vector3 targetLocation;
    public Vector3 currentLocation;
    public Animator avatarAnimator;
    private double lastGpsUpdateTime;
    private float animatorDelta;
    private float animatorTargetDelta;
    private float lastMapViewUpdateTime;
    private const float WalkThreshold = 0.5f;
    private const float RunThreshold = 10f;
    private const float SprintThreshold = 20f;
    private const float TeleportThreshold = 200f;

    private void Awake()
    {
        Instance = this;
    }

    void HandleAvatarAnimation(float delta)
    {
        animatorTargetDelta = delta;
    }

    void HandleAvatarAnimationVertical()
    {
        if (avatarAnimator == null)
        {
            return;
        }

        animatorDelta = Mathf.Lerp(animatorDelta, animatorTargetDelta, 15 * Time.deltaTime);
        avatarAnimator.SetFloat("Vertical", animatorDelta);
    }

    void HandleAvatarMove()
    {
        var movementVector = targetLocation - currentLocation;
        var movementDistance = movementVector.magnitude;

        switch (movementDistance)
        {
            case > TeleportThreshold:
                currentLocation = targetLocation;
                break;

            case > WalkThreshold:
                {
                    // If the player is not stationary,
                    // rotate to face their movement vector
                    var forward = movementVector.normalized;
                    var rotation = Quaternion.LookRotation(forward, Vector3.up);
                    transform.rotation = rotation;
                    break;
                }
        }

        currentLocation = Vector3.Lerp(
            currentLocation,
            targetLocation,
            Time.deltaTime);

        transform.position = currentLocation;

        switch (movementDistance)
        {
            case > SprintThreshold:
                {
                    HandleAvatarAnimation(1f);
                    break;
                }
            case > RunThreshold:
                {
                    HandleAvatarAnimation(0.75f);
                    break;
                }
            case > WalkThreshold:
                {
                    HandleAvatarAnimation(0.5f);
                    break;
                }
            default:
                HandleAvatarAnimation(0);
                break;
        }
    }

    private IEnumerator UpdatePlayerLocation()
    {
        if (Application.isEditor)
        {
            while (isActiveAndEnabled)
            {
                HandleUpdateEditorInput();
                yield return null;
            }
        }
        else
        {

#if UNITY_ANDROID
            // Request location permission for Android
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                {
                    // Wait until permission is enabled
                    yield return new WaitForSeconds(1.0f);
                }
            }
#endif
            while (isActiveAndEnabled)
            {
                var gpsInfo = Input.location.lastData;
                if (gpsInfo.timestamp > lastGpsUpdateTime)
                {
                    lastGpsUpdateTime = gpsInfo.timestamp;
                    var location = GeolocationInput.GetCurrentLocation();
                    targetLocation = map.LatLngToScene(location);
                }

                yield return null;
            }


        }

    }

    private void Start()
    {
        map.MapOriginChanged += OnOriginChanged;
        currentLocation = targetLocation = transform.position;

        StartCoroutine(UpdatePlayerLocation());
    }

    private void HandleUpdateEditorInput()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Make Editor movement relative to the camera's forward direction
        var cameraForward = Vector3.Scale(mapCamera.transform.forward, new Vector3(1, 0, 1));
        var cameraRight = mapCamera.transform.right;
        Vector3 targetMove = cameraForward * vertical + cameraRight * horizontal;


        targetLocation += targetMove * (60 * Time.deltaTime);
    }

    private void OnOriginChanged(LatLng center)
    {
        var offset = targetLocation - currentLocation;
        currentLocation = map.LatLngToScene(center);
        targetLocation = currentLocation + offset;
        transform.position = currentLocation;
    }

    private void Update()
    {
        UpdateMapViewPosition();
        HandleAvatarAnimationVertical();
        HandleAvatarMove();
    }

    private void UpdateMapViewPosition()
    {
        // Only update the map tile view periodically so as not to spam tile fetches
        if (Time.time < lastMapViewUpdateTime + 1.0f)
        {
            return;
        }

        lastMapViewUpdateTime = Time.time;
        map.SetMapCenter(transform.position);
    }
}
