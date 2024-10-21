using UnityEngine;

public class BonkEvent : SpawnableEvent
{
    public GameObject bonkObject;
    public override GameObject spawnableObject()
    {
        return bonkObject;
    }
}
