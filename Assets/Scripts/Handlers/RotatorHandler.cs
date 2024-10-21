using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorHandler : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
    }
}
