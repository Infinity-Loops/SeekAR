using UnityEngine;

public class FlokiEvent : SpawnableEvent
{
    public GameObject flokiObject;
    public override GameObject spawnableObject()
    {
        return flokiObject;
    }
}
