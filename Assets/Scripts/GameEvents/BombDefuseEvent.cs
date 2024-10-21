using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDefuseEvent : SpawnableEvent
{
    public GameObject bombObject;
    public override GameObject spawnableObject()
    {
        return bombObject;
    }
}
