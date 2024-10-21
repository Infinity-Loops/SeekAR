using Niantic.Lightship.Maps.Core.Coordinates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeolocationInput
{
    public static Vector2 initialLocation = Vector2.zero;

    public static void Init()
    {
        Input.location.Start();
        Input.compass.enabled = true;
    }

    public static void RecordInitialLocation()
    {
        if (initialLocation == Vector2.zero)
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                var geoData = Input.location.lastData;
                initialLocation = new Vector2(geoData.latitude, geoData.longitude);
            }
        }
    }

    public static Vector2 GetCenterLocation => GetCurrentLocation(Vector2.zero);

    public static Vector2 GetCurrentLocation(Vector2 lastLocation)
    {
        float lat = lastLocation.x;
        float lon = lastLocation.y;

        if (Input.location.status == LocationServiceStatus.Running)
        {
            var lastReceivedData = Input.location.lastData;
            lat = lastReceivedData.latitude;
            lon = lastReceivedData.longitude;
        }

        return new Vector2(lat, lon);
    }
    public static LatLng GetCurrentLocation(LatLng lastLocation)
    {
        double lat = lastLocation.Latitude;
        double lon = lastLocation.Longitude;

        if (Input.location.status == LocationServiceStatus.Running)
        {
            var lastReceivedData = Input.location.lastData;
            lat = lastReceivedData.latitude;
            lon = lastReceivedData.longitude;
        }

        return new LatLng(lat, lon);
    }
    public static LatLng GetCurrentLocation()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            var lastReceivedData = Input.location.lastData;
            double lat = lastReceivedData.latitude;
            double lon = lastReceivedData.longitude;
            return new LatLng(lat, lon);
        }
        else
        {
            return default;
        }
    }
}
