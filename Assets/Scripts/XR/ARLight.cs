using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARLight : MonoBehaviour
{
    float latitude;
    float longitude;

    private void Update()
    {
#if UNITY_EDITOR
        latitude = -14.2133546f;
        longitude = -39.5223948f;
#else
        latitude = (float)GeolocationInput.GetCenterLocation.x;
        longitude = (float)GeolocationInput.GetCenterLocation.y;
#endif
        var orientation = Quaternion.Euler(CalculateSunOrientation());
        orientation.Normalize();

        transform.rotation = orientation;
    }

    public Vector3 CalculateSunOrientation()
    {
        DateTime now = DateTime.Now;

        double longitudeHour = (now.Hour * 60 + now.Minute) * 0.25 - 180;
        double localHourAngle = longitudeHour + longitude;

        double julianDay = 367 * now.Year - (int)((7.0 / 4.0) * (now.Year + ((now.Month + 9) / 12.0)))
            + now.Day - ((int)(275 * now.Month / 9.0)) + 1721013.5;
        double time = (now.Hour * 3600 + now.Minute * 60 + now.Second) / 86400.0;
        double meanAnomaly = 357.5291 + 0.98560028 * (julianDay - 2451545) + 360.0 * time;
        double solarDeclination =
            Math.Asin(Math.Sin(23.439292 * Math.PI / 180) * Math.Sin(meanAnomaly * Math.PI / 180));

        double solarElevation = Math.Asin(Math.Sin(latitude * Math.PI / 180) * Math.Sin(solarDeclination)
                                          + Math.Cos(latitude * Math.PI / 180) * Math.Cos(solarDeclination) *
                                          Math.Cos(localHourAngle * Math.PI / 180));

        double solarAzimuth = Math.Acos(
            (Math.Sin(solarDeclination) - Math.Sin(latitude * Math.PI / 180) * Math.Sin(solarElevation))
            / (Math.Cos(latitude * Math.PI / 180) * Math.Cos(solarElevation)));

        solarElevation = solarElevation * 180 / Math.PI;
        solarAzimuth = solarAzimuth * 180 / Math.PI;

        Vector3 sunRotation = new Vector3((float)solarElevation, (float)solarAzimuth, 0f);

        return sunRotation;
    }
}