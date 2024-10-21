using System.Collections.Generic;
using UnityEngine;

public class PowerupsEvent : MonoBehaviour
{
    public Vector3 spawnRange;
    public float lookAngle = -180f;
    public float spawnRate = 5f;
    public int maxInstances = 10;
    public List<GameObject> powerups = new List<GameObject>();
    private List<PowerupEventInstance> instances = new List<PowerupEventInstance>();

    void CreatePowerUpInstances()
    {
        foreach (var powerup in powerups)
        {
            var eventInstance = gameObject.AddComponent<PowerupEventInstance>();
            eventInstance.refer = powerup;
            eventInstance.lookAngle = lookAngle;
            eventInstance.spawnRange = spawnRange;
            eventInstance.spawnRate = spawnRate;
            eventInstance.maxInstances = maxInstances;
            instances.Add(eventInstance);
        }
    }

    private void Start()
    {
        CreatePowerUpInstances();
    }
}

public class PowerupEventInstance : SpawnableEvent
{
    public GameObject refer;
    public override GameObject spawnableObject()
    {
        return refer;
    }
}