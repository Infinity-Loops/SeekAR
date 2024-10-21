using DG.Tweening;
using Niantic.Lightship.Maps.Core.Coordinates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapToDropCoinCharacter : MonoBehaviour, IPointerDownHandler
{
    public Animator animator;
    public string dropAnimation;
    public Transform coinCustomAnimBone;
    public float coinCustomAnimBoneScale = 1f;
    public float timeToDrop;
    public GameObject coinToDrop;
    public GameObject endSequenceEffect;
    private bool sequenceStarted;
    private bool isCloseEnough;
    private GameObject instantiatedMapObject;

    IEnumerator HandleTapToDropSequence()
    {
        if(coinCustomAnimBone != null)
        {
            animator.Play(dropAnimation);
            yield return new WaitForEndOfFrame();
            Vector3 targetCoinLocation = transform.position + (transform.forward * 1.5f);
            var coinTransform = Instantiate(coinToDrop, transform.position, Quaternion.identity).transform;
            var cda = coinTransform.gameObject.AddComponent<CoinDropAnimator>();
            cda.bone = coinCustomAnimBone;
            cda.scaler = coinCustomAnimBoneScale;
            yield return new WaitForSeconds(timeToDrop);
            Instantiate(endSequenceEffect, transform.position, Quaternion.identity);
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
        else
        {
            animator.Play(dropAnimation);

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(timeToDrop);
            Vector3 targetCoinLocation = transform.position + (transform.forward * 1.5f);
            var coinTransform = Instantiate(coinToDrop, transform.position, Quaternion.identity).transform;
            coinTransform.DOMove(targetCoinLocation, 1f);
            yield return new WaitForSeconds(1f);
            Instantiate(endSequenceEffect, transform.position, Quaternion.identity);
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }

    }

    void HandleRotation()
    {
        float distance = Vector3.Distance(ARCamera.arCamera.transform.position, transform.position);
        if (distance <= 5)
        {
            if (!isCloseEnough)
            {
                isCloseEnough = true;
                transform.DORotate(new Vector3(0, ARCamera.arCamera.transform.eulerAngles.y - 180, 0), 0.25f);
            }
        }
        else
        {
            isCloseEnough = false;
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
    }

    //void HandleSpawnMapRepresentation()
    //{
    //    instantiatedMapObject = Instantiate(mapObject);
    //    Vector3 objectPosition = transform.position;
    //    LatLng worldPos = MapSystem.instance.map.SceneToLatLng(objectPosition);
    //    Vector3 targetLocation = MapSystem.instance.map.LatLngToScene(worldPos);

    //    instantiatedMapObject.transform.localScale = Vector3.one * 10f;
    //    instantiatedMapObject.transform.position = targetLocation + Vector3.up * 10f;
    //}

    private void Awake()
    {
        //HandleSpawnMapRepresentation();
    }

    private void Update()
    {
        HandleRotation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!sequenceStarted)
        {
            sequenceStarted = true;
            StartCoroutine(HandleTapToDropSequence());
        }
    }

    private void OnDestroy()
    {
        if (instantiatedMapObject != null)
        {
            Destroy(instantiatedMapObject);
        }
    }
}

public class CoinDropAnimator : MonoBehaviour
{
    public Transform bone;
    public float scaler = 1f;
    private Vector3 scale;
    private void Start()
    {
        scale = transform.localScale;
    }
    private void Update()
    {
        if(bone != null)
        {
            transform.localScale = bone.localScale * scaler;
            transform.position = bone.position;
        }
    }
}