using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeolocationUtils
{   
    // Origin shift constant used to convert lat/lon to Mercator projection (in meters)
    private const double OriginShift = 2 * Math.PI * 6378137 / 2.0;

    public static Vector2 WorldToGeoPosition(double x, double y, Vector2 refPoint, float scale = 1)
    {

        const double OriginShift = 2 * Math.PI * 6378137 / 2.0;

        double lon = ((x / scale) + refPoint.x) * 180 / OriginShift;

        double posy = ((y / scale) + refPoint.y) * 180 / OriginShift;
        double lat = 180 / Math.PI * (2 * Math.Atan(Math.Exp(posy * Math.PI / 180)) - Math.PI / 2);

        return new Vector2((float)lat, (float)lon);
    }

    /// <summary>
    /// Converts WGS84 latitude/longitude to Unity world position (x/y) relative to a reference point in Mercator coordinates.
    /// </summary>
    /// <param name="latitude">The latitude in WGS84.</param>
    /// <param name="longitude">The longitude in WGS84.</param>
    /// <param name="referencePointMercator">Reference point in Mercator coordinates.</param>
    /// <param name="scale">Scale factor in meters (default = 1 meter per Unity unit).</param>
    /// <returns>A <see cref="Vector2"/> representing the Unity world position in x and y.</returns>
    public static Vector2 LatLonToUnityPosition(double latitude, double longitude, Vector2 referencePointMercator, float scale = 1f)
    {
        // Convert lat/lon to Mercator projection (in meters)
        double mercatorX = longitude * OriginShift / 180;
        double mercatorY = Math.Log(Math.Tan((90 + latitude) * Math.PI / 360)) * OriginShift / 180;

        // Apply reference point and scaling
        float unityX = (float)((mercatorX - referencePointMercator.x) * scale);
        float unityY = (float)((mercatorY - referencePointMercator.y) * scale);

        // Return the position in Unity coordinates
        return new Vector2(unityX, unityY);
    }

    /// <summary>
    /// Converts WGS84 latitude/longitude to Mercator projection coordinates.
    /// </summary>
    /// <param name="latitude">Latitude in WGS84.</param>
    /// <param name="longitude">Longitude in WGS84.</param>
    /// <returns>A <see cref="Vector2d"/> representing the Mercator coordinates in meters.</returns>
    public static Vector2 LatLonToMercator(double latitude, double longitude)
    {
        double mercatorX = longitude * OriginShift / 180;
        double mercatorY = Math.Log(Math.Tan((90 + latitude) * Math.PI / 360)) * OriginShift / 180;

        return new Vector2((float)mercatorX, (float)mercatorY);
    }
}
