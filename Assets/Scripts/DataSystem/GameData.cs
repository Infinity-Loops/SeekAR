using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public PlayerData playerData = new PlayerData();
    public bool isBetaAuthScreenPassed;
    public bool isDynatrixInitialAudioPlayed;
    public bool isHealthTabInitialAudioPlayed;
    public bool isSettingsTabInitialAudioPlayed;
    public bool isMessageTabInitialAudioPlayed;
    public bool isFriendsTabInitialAudioPlayed;
    public bool isMapTabInitialAudioPlayed;
    public bool isStakeAndSwapTabInitialAudioPlayed;
    public bool dontRememberARWarn;
}
