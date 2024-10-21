using UnityEngine;

public class DogeEvent : SpawnableEvent
{
    public GameObject dogeObject;
    public override GameObject spawnableObject()
    {
        return dogeObject;
    }
}
