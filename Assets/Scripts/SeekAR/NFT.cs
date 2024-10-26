
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
    private bool isOpen;
    public void OnOpen()
    {
        var randomNFT = nfts[Random.Range(0, nfts.Count)];

        var instantedNFT = Instantiate(randomNFT, instancePole);
        Instantiate(particle, instancePole.position, Quaternion.identity);
        isOpen = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!sequenceStarted)
        {
            sequenceStarted = true;
            animator.Play("Open");
        }
        else if (isOpen)
        {
            CollectionNotificationHandler.instance.HandleNotificationSequence(null, null, 1, "SEEKARNFT", "NFT");
            Destroy(gameObject);
        }
    }
}
