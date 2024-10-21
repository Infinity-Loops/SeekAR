using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CutWire : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onWireCut;
    public void OnPointerDown(PointerEventData eventData)
    {
        onWireCut.Invoke();
        transform.localScale = Vector3.zero;
    }
}
