using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RemoveBattery : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onRemove;

    public void OnPointerDown(PointerEventData eventData)
    {
        onRemove.Invoke();
        transform.DOLocalMove(new Vector3(-1, transform.localPosition.y, transform.localPosition.z), 0.25f).OnComplete(() =>
        {
            transform.DOScale(Vector3.zero, 0.25f);
        });
    }
}
