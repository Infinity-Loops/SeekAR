using UnityEngine;
using UnityEngine.UI;

public class SyncedSpriteImage : MonoBehaviour
{
    public Image imageToCopy;
    public Image image;

    private void Update()
    {
        image.sprite = imageToCopy.sprite;
    }
}
