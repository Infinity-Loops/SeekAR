using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthScreen : MonoBehaviour
{
    private const string betaToken = "B9BJO-HVCI8-YOX63-UXH4V-HF007";
    public GameObject tokenPopup;
    public GameObject authContent;
    public TMP_InputField tokenInput;
    public TMP_InputField emailInput;
    public TMP_InputField nameInput;
    public CanvasGroup errorNotification;
    public GameObject loadingScrene;
    public GameObject afterLoading;

    public void ConfirmAccessTokenMessage()
    {
        tokenPopup.SetActive(false);
        authContent.SetActive(true);
    }

    void HandleErrorNotification()
    {
        errorNotification.gameObject.SetActive(true);
        errorNotification.alpha = 0;
        errorNotification.DOFade(1f, 0.25f).OnComplete(() =>
        {
            errorNotification.DOFade(0f, 0.25f).SetDelay(3f).OnComplete(() =>
            {
                errorNotification.gameObject.SetActive(false);
            });
        });
    }

    public void NextButton()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            HandleErrorNotification();
            return;
        }

        if (string.IsNullOrEmpty(emailInput.text))
        {
            HandleErrorNotification();
            return;
        }

        if (tokenInput.text == betaToken)
        {
            DataSystem.gameData.playerData.playerName = nameInput.text;
            DataSystem.gameData.playerData.playerEmail = emailInput.text;
            DataSystem.gameData.isBetaAuthScreenPassed = true;
            DataSystem.SaveData();
            StartCoroutine(HandleNextSequence());
        }
        else
        {
            HandleErrorNotification();
        }
    }

    IEnumerator HandleNextSequence()
    {
        loadingScrene.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        loadingScrene.gameObject.SetActive(false);
        afterLoading.gameObject.SetActive(true);
        SceneManager.LoadSceneAsync("GameScene");
    }
}
