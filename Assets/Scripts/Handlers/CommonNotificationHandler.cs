using DG.Tweening;
using TMPro;
using UnityEngine;

public class CommonNotificationHandler : MonoBehaviour
{
    public static CommonNotificationHandler instance;
    public TMP_Text notificationText;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        instance = this;
    }

    public void HandleNotificationSequence(string text, float duration)
    {
        notificationText.text = text;
        rectTransform.DOAnchorPosY(-255f, 0.25f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            rectTransform.DOAnchorPosY(255f, 0.25f).SetDelay(duration);
        });
    }
}
