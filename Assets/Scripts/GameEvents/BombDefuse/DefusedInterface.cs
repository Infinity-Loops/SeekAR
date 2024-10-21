using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefusedInterface : MonoBehaviour
{
    public Image icon;
    public Nft choosenReward;
    public void CollectReward()
    {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            Quaternion rotation = ARCamera.arCamera.transform.rotation * Quaternion.Euler(0, -180, 0);
            Instantiate(choosenReward.collectablePrefab, transform.position, rotation);
            Destroy(transform.parent.gameObject);
        });
    }
}
