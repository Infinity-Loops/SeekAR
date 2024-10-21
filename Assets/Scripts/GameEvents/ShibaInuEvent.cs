using UnityEngine;

public class ShibaInuEvent : SpawnableEvent
{
    public GameObject shibaInuObject;
    public override GameObject spawnableObject()
    {
        return shibaInuObject;
    }
}
