using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PadButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent<string> OnPress = new UnityEvent<string>();
    public string padCharacter;

    private Vector3 startPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 location = new Vector3(startPosition.x, startPosition.y, 0.032f);
        transform.DOLocalMove(location, 0.25f).OnComplete(() =>
        {
            transform.DOLocalMove(startPosition, 0.25f);
        });

        OnPress.Invoke(padCharacter);
    }

    private void Awake()
    {
        startPosition = transform.localPosition;
    }
}
