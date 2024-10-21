using Niantic.Lightship.Maps.Core.Coordinates;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableEvent : MonoBehaviour, ISpawnableEvent
{
    public Vector3 spawnRange;
    public float lookAngle = -180f;
    public float spawnRate = 5f;
    public int maxInstances = 10;
    private float spawnTime;
    private List<GameObject> instances = new List<GameObject>();
    void HandleSpawnEvent()
    {
        if (Time.time > spawnTime)
        {
            spawnTime = Time.time + spawnRate;

            Vector3 targetLocation = ARCamera.arCamera.transform.position;
            LatLng worldPos = MapSystem.instance.map.SceneToLatLng(targetLocation);
            targetLocation = MapSystem.instance.map.LatLngToScene(worldPos);

            Quaternion rotation = ARCamera.arCamera.transform.rotation * Quaternion.Euler(0, lookAngle, 0);
            targetLocation += new Vector3(Random.Range(-spawnRange.x, spawnRange.x), targetLocation.y, Random.Range(-spawnRange.y, spawnRange.y));

            if (instances.Count >= maxInstances)
            {
                foreach (GameObject instance in instances)
                {
                    Destroy(instance);
                }

                instances.Clear();
            }
            var obj = Instantiate(spawnableObject(), targetLocation, Quaternion.identity);
            obj.transform.localScale = Vector3.one * 10f;
            obj.transform.position = targetLocation + Vector3.up * 10f;
            instances.Add(obj);
        }
    }

    private void Update()
    {
        HandleSpawnEvent();
    }

    public virtual GameObject spawnableObject()
    {
        return null;
    }
}

public interface ISpawnableEvent
{
    public GameObject spawnableObject();
}