using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectableCoin : MonoBehaviour, IPointerDownHandler
{
    public GameObject pickUpEffect;
    public bool disableRotation;
    public AudioClip collectSound;
    private Transform meshRoot;
    internal GameCoin coin;
    private void Awake()
    {
        meshRoot = transform.GetChild(0);
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
        float multiplier = 1f;

        if (PowerupData.pumpItActive)
        {
            multiplier += 4f;
        }

        if (PowerupData.fitnessMultiplierActive)
        {
            double steps = 100;

            steps = HealthManager.steps;

            multiplier += Mathf.Round((float)(steps / 1000));
        }


        CollectionNotificationHandler.instance.HandleNotificationSequence(coin.icon,$"1 {coin.token} = {coin.value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"))}",coin.value * multiplier, coin.name, "");
        CoinTracking.AddCoinValue(coin.name, coin.value * multiplier);
        if (pickUpEffect != null)
        {
            Instantiate(pickUpEffect, meshRoot.position, Quaternion.identity);
        }
        else
        {
            Instantiate(coin.pickupEffect, meshRoot.position, Quaternion.identity);
        }

        if (collectSound != null)
        {
            AudioUtils.PlayClipAtPoint(collectSound, transform.position);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleUpdateRotation();
    }
}
