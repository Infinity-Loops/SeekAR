using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionNotificationHandler : MonoBehaviour
{
    public static CollectionNotificationHandler instance;
    public TMP_Text notificationText;
    public TMP_Text tokenDataText;
    public Image icon;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        instance = this;
    }

    public void HandleNotificationSequence(Sprite sprite,string tokenData, float quantity, string item, string type)
    {
        notificationText.text = $"Collected {quantity}x {item} {type}";
        tokenDataText.text = tokenData;
        rectTransform.DOAnchorPosY(-580f, 0.25f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            rectTransform.DOAnchorPosY(255f, 0.25f).SetDelay(2f);
        });
        icon.sprite = sprite;
    }
}
