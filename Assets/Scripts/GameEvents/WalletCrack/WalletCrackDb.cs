
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WalletCrackDb : ScriptableObject
{
    public List<CrackWordSession> sessions = new List<CrackWordSession>();
}

[System.Serializable]
public class CrackWordSession
{
    public string hint;
    public List<string> words = new List<string>();
}