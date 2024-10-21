using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpinnerHandler : MonoBehaviour
{
    void HandleRotate()
    {
        transform.Rotate(-Vector3.forward * 200 * Time.deltaTime);
    }

    void Update()
    {
        HandleRotate();
    }
}
