using AssetKits.ParticleImage;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PowerupNotificationHandler : MonoBehaviour
{
    public static PowerupNotificationHandler instance;
    public TMP_Text notificationText;
    public TMP_Text titleText;
    public ParticleImage particle;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        instance = this;
    }

    public void HandleNotificationSequence(string title, string text, float duration)
    {
        notificationText.text = text;
        titleText.text = title;
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.DOAnchorPosY(-Screen.height / 2, 0.25f).SetEase(Ease.InOutElastic).OnComplete(() =>
        {
            rectTransform.DOShakeAnchorPos(0.5f, 25f);
            particle.Play();
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.DOAnchorPosY(560.38f, 0.25f).SetDelay(duration);
        });
    }
}
