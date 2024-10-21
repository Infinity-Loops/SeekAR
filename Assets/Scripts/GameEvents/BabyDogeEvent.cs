using UnityEngine;

public class BabyDogeEvent : SpawnableEvent
{
    public GameObject babyDogeObject;

    public override GameObject spawnableObject()
    {
        return babyDogeObject;
    }
}
