using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;
using Niantic.Lightship.Maps;
using Niantic.Lightship.Maps.Core.Coordinates;


public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;
#if UNITY_EDITOR
    public DebugLocation debugLocation;
#endif
    public LatLng geolocation;
    public AudioClip welcomeAudio;
    public LightshipMapView map;
    public Transform player;
    public GameObject mapInterface;
    public GameObject mapCamera;
    public float zoom;
    private bool mapActive;
    private float lastMapViewUpdateTime;

    public void EnableOrDisableMap()
    {
        if (!mapActive)
        {
            mapActive = true;
            mapCamera.gameObject.SetActive(true);
            //ARCamera.arCamera.enabled = false;
            mapInterface.SetActive(true);
            HandleMapWelcomeAudio();
            return;
        }

        if (mapActive)
        {
            mapActive = false;
           mapCamera.gameObject.SetActive(false);
            mapInterface.SetActive(false);
            //ARCamera.arCamera.enabled = true;
            return;
        }
    }
    public void DisableMap()
    {
        mapActive = false;
        mapCamera.gameObject.SetActive(false);
        mapInterface.SetActive(false);
    }
    public void EnableMap()
    {
        mapActive = true;
        mapCamera.gameObject.SetActive(true);
        //ARCamera.arCamera.enabled = false;
        mapInterface.SetActive(true);
        HandleMapWelcomeAudio();
    }
    public void ZoomIn()
    {
        zoom -= 25 * Time.deltaTime;
    }
    public void ZoomOut()
    {
        zoom += 25 * Time.deltaTime;
    }
    void HandleMapWelcomeAudio()
    {
        if (!DataSystem.gameData.isMapTabInitialAudioPlayed)
        {
            AudioUtils.PlayClipAtPoint(welcomeAudio, transform.position, 1f, 0f);
            DataSystem.gameData.isMapTabInitialAudioPlayed = true;
            DataSystem.SaveData();
        }
    }


    private void Awake()
    {
        instance = this;

#if UNITY_EDITOR
        geolocation = new LatLng(debugLocation.latitude, debugLocation.longitude);
#endif

    }


    private void Update()
    {
        UpdateGPS();
    }

    private void UpdateGPS()
    {
        geolocation = GeolocationInput.GetCurrentLocation(geolocation);
    }

}

#if UNITY_EDITOR
[Serializable]
public class DebugLocation
{
    public double latitude;
    public double longitude;
}
#endif