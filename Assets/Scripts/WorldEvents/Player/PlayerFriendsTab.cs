using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFriendsTab : MonoBehaviour
{
    public AudioClip welcomeFriendsTab;

    void HandleFriendsTabWelcomeAudio()
    {
        if (!DataSystem.gameData.isFriendsTabInitialAudioPlayed)
        {
            AudioUtils.PlayClipAtPoint(welcomeFriendsTab, transform.position);
            DataSystem.gameData.isFriendsTabInitialAudioPlayed = true;
            DataSystem.SaveData();
        }
    }

    private void OnEnable()
    {
        HandleFriendsTabWelcomeAudio();
    }
}
