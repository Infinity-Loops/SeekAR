using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogwifhatEvent : SpawnableEvent
{
    public GameObject dogwifhatObject;
    public override GameObject spawnableObject()
    {
        return dogwifhatObject;
    }
}
