using UnityEngine;

public class MapSkybox : MonoBehaviour
{
    public Transform camera;
    public Light directionalLight;
    public MeshRenderer skyRenderer;
    private Material skyMaterialInstance;
    private void Awake()
    {
        skyMaterialInstance = skyRenderer.material;
    }

    void HandleSkyboxUpdate()
    {
        skyMaterialInstance.SetVector("_LightDir", directionalLight.transform.localRotation * Vector3.forward * 0.1f);
        skyRenderer.sharedMaterial = skyMaterialInstance;
    }
    void HandlePositionUpdate()
    {
        transform.position = new Vector3(camera.position.x, 0, camera.position.z);
    }

    private void Update()
    {
        HandleSkyboxUpdate();
        HandlePositionUpdate();
    }
}
