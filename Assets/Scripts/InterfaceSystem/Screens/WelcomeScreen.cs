using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScreen : MonoBehaviour
{
    public Transform spinningLoadingIndicator;
    public Transform initialTerms;
    public Transform logo;
    public TMP_Text termsText;

    IEnumerator InitialSequence()
    {
        yield return new WaitForSeconds(1);
        spinningLoadingIndicator.gameObject.SetActive(true);
        //SceneManager.LoadSceneAsync("GameScene");
        if (DataSystem.gameData.isBetaAuthScreenPassed)
        {
            SceneManager.LoadSceneAsync("GameScene");
        }
        else
        {
            yield return new WaitForSeconds(2.5f);
            spinningLoadingIndicator.gameObject.SetActive(false);
            logo.gameObject.SetActive(false);
            var termsFader = initialTerms.GetComponent<CanvasGroup>();
            termsFader.DOFade(1f, 0.5f).OnComplete(() =>
            {
                termsFader.interactable = true;
            });
        }

    }

    public void AcceptTermsAndContinue()
    {
        initialTerms.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            var ISM = GetComponentInParent<InterfaceSequenceManager>();
            ISM.GoNextInterface();
        });

    }

    void HandleTermsDate()
    {
        termsText.text = termsText.text.Replace("[Date]", $"{DateTime.UtcNow.Day.ToString("00")}/{DateTime.UtcNow.Month.ToString("00")}/{DateTime.UtcNow.Year}");
    }

    private void Start()
    {
        HandleTermsDate();
        StartCoroutine(InitialSequence());
    }

    private void Update()
    {
        if (spinningLoadingIndicator.gameObject.activeInHierarchy)
        {
            spinningLoadingIndicator.transform.Rotate(-Vector3.forward * 200 * Time.deltaTime);
        }
    }
}
