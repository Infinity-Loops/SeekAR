using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerName;
    public string playerEmail;
    public string playerAvatar;
    public string avatarUrl;
    public string readyPlayerMeAuthToken;
    public int playerLevel;
    public int playerXP;
    public List<CurrencyData> playerBalance = new List<CurrencyData>();
}

[Serializable]
public class CurrencyData
{
    public string currencyID;
    public double currencyBalance;
}