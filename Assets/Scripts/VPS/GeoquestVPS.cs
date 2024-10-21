using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoquestVPS : MonoBehaviour
{
    Vector2 currentMercatorPosition;

    void HandleUpdateCurrentMercatorPosition()
    {
        var currentLocation = GeolocationInput.GetCenterLocation;
        currentMercatorPosition = GeolocationUtils.LatLonToMercator(currentLocation.x, currentLocation.y);
    }

    private void Update()
    {
        HandleUpdateCurrentMercatorPosition();
    }
}
