
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NFT : MonoBehaviour, IPointerDownHandler
{
    private bool sequenceStarted;
    public Animator animator;
    public List<GameObject> nfts = new List<GameObject>();
    public GameObject particle;
    public Transform instancePole;
    public void OnOpen()
    {
        var randomNFT = nfts[Random.Range(0, nfts.Count)];

        var instantedNFT = Instantiate(randomNFT, instancePole);
        instancePole.transform.localScale = Vector3.zero;
        Instantiate(particle, instancePole.position, Quaternion.identity);
        instancePole.DOScale(Vector3.one, 1f);
        instancePole.DOLocalMoveY(0.8f, 1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!sequenceStarted)
        {
            sequenceStarted = true;
            animator.Play("Open");
        }
    }
}
