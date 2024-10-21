using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinShower : MonoBehaviour
{
    public GameObject model;
    public Transform startPole;
    public Vector3 startPos;
    public float drag;
    public float mass;
    private List<Rigidbody> instances = new List<Rigidbody>();

    void HandleInstances()
    {
        var randomRot = Random.insideUnitSphere * 360;
        var randomPos = Random.insideUnitCircle * 0.5f;
        var modelInstance = Instantiate(model, startPole.position + startPos + new Vector3(randomPos.x, 0), Quaternion.Euler(randomRot));

        var renderers = modelInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            var col = renderer.gameObject.AddComponent<MeshCollider>();
            col.convex = true;
            renderer.gameObject.layer = 7;
        }



        var rigidInstance = modelInstance.AddComponent<Rigidbody>();
        rigidInstance.linearDamping = drag;
        rigidInstance.mass = mass;
        Destroy(modelInstance, 10);
    }

    public void Play(int instances)
    {
        StartCoroutine(Spawn(instances));
    }

    IEnumerator Spawn(int instances)
    {
        for (int i = 0; i < instances; i++)
        {
            yield return new WaitForSeconds(Random.value * 0.25f);
            HandleInstances();
        }
    }
}
