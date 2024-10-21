using UnityEngine;

public class BrettEvent : SpawnableEvent
{
    public GameObject brettObject;
    public override GameObject spawnableObject()
    {
        return brettObject;
    }
}
