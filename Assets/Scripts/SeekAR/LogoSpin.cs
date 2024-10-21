using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoSpin : MonoBehaviour
{
    public CanvasGroup introScreen;
    private int rotateInteractionCount;
    public WelcomeScreen screen;
    private void Start()
    {
        Animate();
    }
    void Animate()
    {
        transform.DORotate(new Vector3(0, 0, -20), 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 0, 20), 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                rotateInteractionCount++;

                if(rotateInteractionCount == 3)
                {
                    transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
                    {
                        transform.DOPunchScale(Vector3.one * 1.125f, 0.25f).OnComplete(() =>
                        {
                            introScreen.DOFade(0f, 0.25f).OnComplete(() =>
                            {
                                introScreen.gameObject.SetActive(false);
                                screen.StartCoroutine(screen.InitialSequence());
                            });
                        });
                    });
                }
                else
                {
                    Animate();
                }

            });
        });
    }
}
