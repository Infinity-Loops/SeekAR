using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
public class RandomNFTBoxEvent : MonoBehaviour
{
    public NftDB nfts;
    public float checkInterval;
    private float totalTime;
    private double stepCount;
    private DateTime lastTime;
    void CheckForStepCount()
    {
        if (Time.time > totalTime)
        {
            totalTime = Time.time + checkInterval;

            stepCount = HealthManager.steps;
        }

        if (stepCount >= 1000)
        {
            stepCount = 0;
            lastTime = DateTime.Now;
            OnReachStepTarget();
        }
    }

    void OnReachStepTarget()
    {
        int randomRange = Random.Range(0, nfts.NftList.Count);
        var nft = nfts.NftList[randomRange];
        Vector3 location = ARCamera.arCamera.transform.position + ARCamera.arCamera.transform.forward * 2f;
        Quaternion rotation = Quaternion.Euler(0, ARCamera.arCamera.transform.eulerAngles.y, 0);
        Instantiate(nft.collectablePrefab, location, rotation);
    }


    void Awake()
    {
        lastTime = ClientManager.startTime;
    }

    void Update()
    {
        CheckForStepCount();
    }
}
