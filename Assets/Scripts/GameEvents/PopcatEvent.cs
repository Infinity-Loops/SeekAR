using UnityEngine;

public class PopcatEvent : SpawnableEvent
{
    public GameObject popcatObject;
    public override GameObject spawnableObject()
    {
        return popcatObject;
    }
}
