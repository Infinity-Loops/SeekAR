using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerStakeAndSwapTab : MonoBehaviour
{
    [Header("Shared")]
    public CoinDB coinDatabase;
    public AudioClip welcomeAudio;
    public GameObject globalMenu;
    [Header("Swap")]
    public TMP_InputField fromInput;
    public TMP_InputField toInput;
    public TMP_Dropdown toType;
    public GameObject processing;
    public GameObject swapDone;
    private decimal lastSwapValue;
    [Header("Stake")]
    public TMP_InputField fromStakeInput;
    public TMP_Dropdown fromStakeType;
    public GameObject processingStake;
    public GameObject stakeDone;
    public GameObject stakeMachineModel;

    void HandleWelcomeAudio()
    {
        if (!DataSystem.gameData.isStakeAndSwapTabInitialAudioPlayed)
        {
            AudioUtils.PlayClipAtPoint(welcomeAudio, transform.position);
            DataSystem.gameData.isStakeAndSwapTabInitialAudioPlayed = true;
            DataSystem.SaveData();
        }
    }

    private void OnEnable()
    {
        HandleWelcomeAudio();
    }

    private void Awake()
    {
        HandleAddCoinTypesToDropdowns();
        fromInput.onValueChanged.AddListener(OnChangeInputValue);
        toType.onValueChanged.AddListener(OnSwitchOutputCoin);
    }

    void OnSwitchOutputCoin(int value)
    {
        OnChangeInputValue(fromInput.text);
    }

    void OnChangeInputValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        float coinValue = float.Parse(value);
        var selectedCoin = coinDatabase.coins[toType.value];
        lastSwapValue = (decimal)(coinValue * selectedCoin.conversionValue);
        toInput.text = lastSwapValue.ToString();
    }

    void HandleAddCoinTypesToDropdowns()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < coinDatabase.coins.Count; i++)
        {
            var option = new TMP_Dropdown.OptionData();
            option.text = coinDatabase.coins[i].token;
            option.image = coinDatabase.coins[i].icon;
            options.Add(option);
        }

        toType.AddOptions(options);
        fromStakeType.AddOptions(options);
    }

    public void HandleSwap()
    {
        if (string.IsNullOrEmpty(fromInput.text))
        {
            return;
        }

        float coinValue = float.Parse(fromInput.text);

        if (coinValue <= 0)
        {
            return;
        }

        StartCoroutine(HandleSwapSequence());
    }

    IEnumerator HandleSwapSequence()
    {
        fromInput.text = "";
        toInput.text = "";
        yield return new WaitForSeconds(0.1f);
        processing.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        processing.SetActive(false);
        swapDone.SetActive(true);
        yield return new WaitForSeconds(1f);
        swapDone.SetActive(false);
    }

    public void HandleStake()
    {
        if (string.IsNullOrEmpty(fromStakeInput.text))
        {
            return;
        }

        float coinValue = float.Parse(fromStakeInput.text);

        if (coinValue <= 0)
        {
            return;
        }

        StartCoroutine(HandleStakeSequence());
    }

    IEnumerator HandleStakeSequence()
    {
        fromStakeInput.text = "";
        var currentScale = globalMenu.transform.localScale;
        globalMenu.transform.DOScale(Vector3.zero, 0.25f);
        yield return new WaitForSeconds(0.3f);
        Vector3 location = ARCamera.arCamera.transform.position + (ARCamera.arCamera.transform.forward * 2f) + (Vector3.up * -0.25f);
        Quaternion rotation = ARCamera.arCamera.transform.rotation * Quaternion.Euler(0, -180, 0);
        var localStakeMachine = Instantiate(stakeMachineModel, location, rotation);
        var stakeAnimationHandler = localStakeMachine.GetComponent<CoinStakerAnimationHandler>();
        var selectedCoin = coinDatabase.coins[fromStakeType.value];
        stakeAnimationHandler.coinModel = selectedCoin.model;
        stakeAnimationHandler.HandlePlayOpen();
        yield return new WaitForSeconds(3f);
        localStakeMachine.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            Destroy(localStakeMachine);
        });
        yield return new WaitForSeconds(0.3f);
        globalMenu.transform.DOScale(currentScale, 0.25f);
        yield return new WaitForSeconds(0.3f);
        processingStake.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        processingStake.SetActive(false);
        stakeDone.SetActive(true);
        yield return new WaitForSeconds(1f);
        stakeDone.SetActive(false);
    }
}
