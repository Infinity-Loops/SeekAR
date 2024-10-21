using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageRenderer : MonoBehaviour
{
    private RenderTexture rt;
    public RawImage targetImage;
    public Camera targetCamera;
    private void Awake()
    {
        rt = new RenderTexture(Screen.width, Screen.height, 24);
        rt.format = RenderTextureFormat.ARGB32;

        targetImage.texture = rt;
        targetImage.color = Color.white;
        targetCamera.targetTexture = rt;
    }
}
