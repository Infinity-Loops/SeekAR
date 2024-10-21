using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameMenu : MonoBehaviour
{
    public List<GameObject> tabs = new List<GameObject>();
    public AudioClip welcomeDynatrix;

    public void ShowTab(int index)
    {
        foreach (GameObject go in tabs)
        {
            go.SetActive(false);
        }

        tabs[index].SetActive(true);
    }

    public void Close()
    {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    public void HandleInitialWelcomeAudio()
    {
        if (!DataSystem.gameData.isDynatrixInitialAudioPlayed)
        {
            AudioUtils.PlayClipAtPoint(welcomeDynatrix, transform.position);
            DataSystem.gameData.isDynatrixInitialAudioPlayed = true;
            DataSystem.SaveData();
        }
    }

    private void Awake()
    {
        HandleInitialWelcomeAudio();
    }
}