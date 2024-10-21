using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SwitchButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent<bool> onToggle = new UnityEvent<bool>();
    public bool state;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (state)
        {
            state = false;
            onToggle.Invoke(state);
            transform.DOLocalRotate(new Vector3(0, -90, -90), 0.25f);
            return;
        }

        if (!state)
        {
            state = true;
            onToggle.Invoke(state);
            transform.DOLocalRotate(new Vector3(0, -90, -60), 0.25f);
            return;
        }
    }
}
