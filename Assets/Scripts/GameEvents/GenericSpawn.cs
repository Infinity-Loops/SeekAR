using UnityEngine;

public class GenericSpawn : SpawnableEvent
{
    public GameObject spawnObject;
    public override GameObject spawnableObject()
    {
        return spawnObject;
    }
}
