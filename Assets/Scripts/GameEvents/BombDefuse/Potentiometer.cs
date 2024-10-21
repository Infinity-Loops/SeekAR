using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Potentiometer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float value;
    public bool isTouching;
    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private Vector2 potentiometerDelta;
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouching = true;
        lastPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
        lastPosition = Vector2.zero;
    }

    public void HandlePotentiometer()
    {
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                currentPosition = touch.position;
            }
        }

        potentiometerDelta = (currentPosition - lastPosition).normalized;

        if (isTouching)
        {
            value = Mathf.Clamp(value + potentiometerDelta.x * 0.25f * Time.deltaTime, 0, 1);
        }

        transform.localEulerAngles = new Vector3(360 * value, -90, -90);
    }

    void Update()
    {
        HandlePotentiometer();
    }
}
