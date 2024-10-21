using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMessagesTab : MonoBehaviour
{
    public AudioClip welcomeMessagesTab;

    void HandleMessageTabWelcomeAudio()
    {
        if (!DataSystem.gameData.isMessageTabInitialAudioPlayed)
        {
            AudioUtils.PlayClipAtPoint(welcomeMessagesTab, transform.position);
            DataSystem.gameData.isMessageTabInitialAudioPlayed = true;
            DataSystem.SaveData();
        }
    }

    private void OnEnable()
    {
        HandleMessageTabWelcomeAudio();
    }
}
