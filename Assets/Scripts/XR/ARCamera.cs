using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCamera : MonoBehaviour
{
    public static Camera arCamera;
    private void Awake()
    {
        arCamera = GetComponent<Camera>();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        GeolocationInput.RecordInitialLocation();
    }
}
