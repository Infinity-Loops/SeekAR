using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NftDB : ScriptableObject
{
    public List<Nft> NftList = new List<Nft>();
}

[Serializable]
public class Nft
{
    public string name;
    public Sprite icon;
    public GameObject collectablePrefab;
    public float value;
    public string currency;
}