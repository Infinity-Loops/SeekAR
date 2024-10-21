using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsTab : MonoBehaviour
{
    public AudioClip welcomeSettingsTab;

    void HandleSettingsTabWelcomeAudio()
    {
        if (!DataSystem.gameData.isSettingsTabInitialAudioPlayed)
        {
            AudioUtils.PlayClipAtPoint(welcomeSettingsTab, transform.position);
            DataSystem.gameData.isSettingsTabInitialAudioPlayed = true;
            DataSystem.SaveData();
        }
    }

    private void OnEnable()
    {
        HandleSettingsTabWelcomeAudio();
    }
}
