using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletCrackGame : MonoBehaviour
{
    public WalletCrackDb walletCrackDb;
    public CoinDB coinDB;
    public GameObject welcomeScreen;
    public GameObject hintScreen;
    public TMP_Text hintText;
    public GameObject crackScreen;
    public Transform crackContent;
    public GameObject completeScreen;
    public GameObject completeMark;
    public GameObject completeReward;
    public GameObject walletScreen;
    public Image cryptoCoin;
    public TMP_Text cryptoValue;
    public TMP_Text cryptoName;
    public WalletWordButton crackTemplate;
    public ParticleSystem finishParticle;
    public AudioClip finishSound;
    public AudioClip successRetroSound;
    private CrackWordSession session;
    private List<WalletWordButton> buttons = new List<WalletWordButton>();
    private bool match;
    private GameCoin withdrawlCoin;
    private float withdrawlValue;
    public void HandleStartGame()
    {
        welcomeScreen.SetActive(false);
        hintScreen.SetActive(true);
        HandleHintScreen();
    }

    void HandleCrackWords()
    {
        session = walletCrackDb.sessions[Random.Range(0, walletCrackDb.sessions.Count)];
    }

    void HandleHintScreen()
    {
        hintText.text = $"Hint: {session.hint}";
    }

    public void HandleProceedAfterHint()
    {
        hintScreen.SetActive(false);
        crackScreen.SetActive(true);
        HandleCrackWorldScreen();
    }

    void HandleCrackWorldScreen()
    {
        for (int i = 0; i < 12; i++)
        {
            var button = Instantiate(crackTemplate, crackContent);
            button.gameObject.SetActive(true);
            button.dropdown.onValueChanged.AddListener(x => HandleOnChangeWord(button, x));
            buttons.Add(button);
        }
    }

    void HandleOnChangeWord(WalletWordButton button, int index)
    {
        List<string> currentList = new List<string>();
        foreach (WalletWordButton walletButton in buttons)
        {
            currentList.Add(walletButton.GetWord());
        }

        var sessionWords = session.words;

        match = currentList.Count == sessionWords.Count &&
                !currentList.Except(sessionWords).Any() &&
                !sessionWords.Except(currentList).Any();

        StartCoroutine(AfterWordsMatch());
    }

    IEnumerator AfterWordsMatch()
    {
        yield return new WaitForSeconds(1f);
        if (match)
        {
            crackScreen.SetActive(false);
            completeScreen.SetActive(true);
            completeMark.SetActive(true);
            completeMark.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutBounce);
            AudioUtils.PlayClipAtPoint(successRetroSound, transform.position, 1f, 0f, 1f);
            yield return new WaitForSeconds(1f);
            completeMark.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutBounce).OnComplete(() =>
            {
                completeMark.SetActive(false);
            });
            yield return new WaitForSeconds(0.4f);
            completeReward.SetActive(true);
        }
    }

    public void HandleViewReward()
    {
        completeScreen.SetActive(false);
        walletScreen.SetActive(true);

        withdrawlCoin = coinDB.coins[Random.Range(0, coinDB.coins.Count)];
        withdrawlValue = withdrawlCoin.value * Random.Range(1000, 10000);
        cryptoCoin.sprite = withdrawlCoin.icon;
        cryptoValue.text = withdrawlValue.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
        cryptoName.text = withdrawlCoin.name.ToUpper();
    }

    public void HandleWithdrawl()
    {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            CoinTracking.AddCoinValue(withdrawlCoin.name, withdrawlValue);
            AudioUtils.PlayClipAtPoint(finishSound, transform.position, 1f, 0f, 1f);
            Instantiate(finishParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        });
    }

    private void Awake()
    {
        HandleCrackWords();
    }
}
