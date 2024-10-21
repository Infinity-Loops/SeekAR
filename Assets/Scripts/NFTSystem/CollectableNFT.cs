using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectableNFT : MonoBehaviour, IPointerDownHandler
{
    public string targetNFT;
    public GameObject pickUpEffect;
    public bool disableRotation;
    private Transform meshRoot;
    public NftDB nfts;
    private Nft nft;
    private void Awake()
    {
        meshRoot = transform.GetChild(0);
        HandleSelectNFT();
    }

    void HandleSelectNFT()
    {
        nft = nfts.NftList.Find(x => x.name == targetNFT);
    }

    void HandleUpdateRotation()
    {
        if (!disableRotation)
        {
            meshRoot.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CollectionNotificationHandler.instance.HandleNotificationSequence(null, null, 1, nft.name, "NFT");
        Instantiate(pickUpEffect, meshRoot.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleUpdateRotation();
    }
}