using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinInfoPanel : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text aboutTitle;
    public TMP_Text description;
    public TMP_Text type;
    public TMP_Text solanaContract;
    public Image icon;
    private Transform caller;
    private Transform camera;
    private AudioSource audio;

    private void Awake()
    {
        camera = ARCamera.arCamera.transform;
        audio = gameObject.AddComponent<AudioSource>();
        audio.spatialBlend = 1;
        audio.volume = 1;
    }

    public void SetCaller(Transform caller)
    {
        this.caller = caller;
    }

    public void SetDetails(GameCoin coin)
    {
        title.text = coin.name;
        aboutTitle.text = $"About {coin.name}";
        description.text = coin.description;
        icon.sprite = coin.icon;

        if (coin.notSpawn)
        {
            type.text = "MEMECOIN";
        }
        else
        {
            type.text = "ALTCOIN";
        }

        solanaContract.text = coin.solanaContract;
        HandlePlayInitialAudio(coin.audioDescription);
    }

    void HandlePlayInitialAudio(AudioClip clip)
    {
        if (clip != null)
        {
            audio.PlayOneShot(clip);
        }

    }

    public void CopyContract()
    {
        GUIUtility.systemCopyBuffer = solanaContract.text;
    }

    public void Close()
    {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            caller.GetComponent<CollectableCoinName>().ResetInformationalPanelState();
            Destroy(gameObject);
        });
    }


    void HandleMovementUpdate()
    {
        transform.forward = camera.forward;
    }

    private void LateUpdate()
    {
        //HandleMovementUpdate();
    }

}
