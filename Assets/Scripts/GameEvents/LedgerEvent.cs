using UnityEngine;

public class LedgerEvent : SpawnableEvent
{
    public GameObject ledgerObject;
    public override GameObject spawnableObject()
    {
        return ledgerObject;
    }
}