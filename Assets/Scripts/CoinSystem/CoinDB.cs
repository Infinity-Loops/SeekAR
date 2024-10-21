using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CoinDB : ScriptableObject
{
    public List<GameCoin> coins = new List<GameCoin>();
}

[Serializable]
public class GameCoin
{
    public string name;
    public bool notSpawn;
    public string token;
    public string description;
    public string solanaContract;
    public AudioClip audioDescription;
    public GameObject pickupEffect;
    public Sprite icon;
    public Sprite menuIcon;
    public float value;
    public float conversionValue;
    public GameObject model;
    public Vector3 offset;
    public Color menuColor1;
    public Color menuColor2;
}