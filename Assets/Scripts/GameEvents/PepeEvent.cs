using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepeEvent : SpawnableEvent
{
    public GameObject pepeObject;
    public override GameObject spawnableObject()
    {
        return pepeObject;
    }
}
