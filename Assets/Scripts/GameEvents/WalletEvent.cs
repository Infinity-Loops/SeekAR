using UnityEngine;

public class WalletEvent : SpawnableEvent
{
    public GameObject walletObject;
    public override GameObject spawnableObject()
    {
        return walletObject;
    }
}