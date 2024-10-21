using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengesController : MonoBehaviour
{
    public Transform challengeButton;
    public Transform challengeGrid;
    public Transform challengeList;
    public void OpenChallenge()
    {
        challengeButton.transform.DOPunchScale(Vector3.one * 1.0125f, 0.25f,2).OnComplete(() =>
        {

            challengeGrid.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
            {
                challengeGrid.gameObject.SetActive(false);
            });

            challengeList.transform.localScale = Vector3.zero;
            challengeList.gameObject.SetActive(true);
            challengeList.transform.DOScale(Vector3.one, 0.25f);
        });

    }
}
