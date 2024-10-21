using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStakerAnimationHandler : MonoBehaviour
{
    public Animator animator;
    public string open;
    public string close;
    public GameObject coinModel;
    public Transform coinSupport;

    public void HandlePlayOpen()
    {
        var coin = Instantiate(coinModel, coinSupport);
        coin.transform.localPosition = Vector3.zero;
        coin.transform.localRotation = Quaternion.identity;
        coin.transform.localScale = Vector3.one;
        animator.Play(open);
    }

    public void HandlePlayClose()
    {
        var coin = Instantiate(coinModel, coinSupport);
        coin.transform.localPosition = Vector3.zero;
        coin.transform.localRotation = Quaternion.identity;
        coin.transform.localScale = Vector3.one;
        animator.Play(close);
    }
}
