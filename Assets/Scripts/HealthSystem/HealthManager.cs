using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;
    public static int steps;
    public static float totalDistance;
    public static float totalDistanceKM;
    private static Transform camera;
    private Vector3 previousCameraPosition;
    private float stepLength = 0.7f;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        GameObject healthManager = new GameObject("HealthManager", typeof(HealthManager));
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        Initialize();
    }

    void Initialize()
    {
        camera = ARCamera.arCamera.transform;
    }

    void HandleCalculateSteps()
    {
        Vector3 previousPositionXZ = new Vector3(previousCameraPosition.x, 0, previousCameraPosition.z);
        Vector3 currentPositionXZ = new Vector3(camera.position.x, 0, camera.position.z);

        float distanceTraveled = Vector3.Distance(currentPositionXZ, previousPositionXZ);

        totalDistance += distanceTraveled;

        totalDistanceKM = totalDistance / 1000f;

        steps = Mathf.FloorToInt(totalDistance / stepLength);

        previousCameraPosition = camera.position;
    }

    private void Update()
    {
        HandleCalculateSteps();
    }
}
