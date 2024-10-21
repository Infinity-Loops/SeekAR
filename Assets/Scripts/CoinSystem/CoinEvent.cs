using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEvent : SpawnableEvent
{
    public GameObject coinObject;
    public override GameObject spawnableObject()
    {
        return coinObject;
    }
}

