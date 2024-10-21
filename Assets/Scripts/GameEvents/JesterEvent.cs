using UnityEngine;

public class JesterEvent : SpawnableEvent
{
    public GameObject jesterObject;
    public override GameObject spawnableObject()
    {
        return jesterObject;
    }
}
