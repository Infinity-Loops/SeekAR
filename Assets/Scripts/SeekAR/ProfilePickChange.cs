using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePickChange : MonoBehaviour
{
    public Image avatarIcon;

    private void Awake()
    {
        LoadProfilePhoto();
    }
    void LoadProfilePhoto()
    {
        if (DataSystem.avatar != null)
        {
            avatarIcon.sprite = DataSystem.avatar;
        }
    }

    public void ReplacePhoto()
    {
        if (NativeGallery.IsMediaPickerBusy())
        {
            return;
        }

        RequestPermissionAsynchronously(() =>
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                if (path == null)
                {
                    Debug.Log("Operation cancelled");
                }
                else
                {
                    Texture2D imageParsed = NativeGallery.LoadImageAtPath(path);
                    DataSystem.avatar = Sprite.Create(imageParsed, new Rect(Vector2.zero, new Vector2(imageParsed.width, imageParsed.height)), new Vector2(0.5f, 0.5f));
                    avatarIcon.sprite = DataSystem.avatar;

                    DataSystem.SaveAvatarPhoto();
                }

            });


        }, NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

    }

    Texture2D CutTextureToSquare(Texture2D source, int newSize)
    {
        int xOffset = (source.width - newSize) / 2;
        int yOffset = (source.height - newSize) / 2;

        // Cria uma nova textura com a área cortada no centro
        Texture2D croppedTexture = new Texture2D(newSize, newSize);
        Color[] pixels = source.GetPixels(xOffset, yOffset, newSize, newSize);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply(false, false);

        return croppedTexture;
    }

    Texture2D ApplyCircleMask(Texture2D squareTexture)
    {
        int radius = squareTexture.width / 2;
        Texture2D circularTexture = new Texture2D(squareTexture.width, squareTexture.height);

        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < circularTexture.height; y++)
        {
            for (int x = 0; x < circularTexture.width; x++)
            {
                Vector2 pixelPos = new Vector2(x, y);
                float dist = Vector2.Distance(pixelPos, center);

                if (dist <= radius)
                {
                    // Mantém o pixel original dentro do círculo
                    circularTexture.SetPixel(x, y, squareTexture.GetPixel(x, y));
                }
                else
                {
                    // Fora do círculo, define como transparente
                    circularTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
        }

        circularTexture.Apply(false, false);
        return circularTexture;
    }

    private async void RequestPermissionAsynchronously(Action onFinish, NativeGallery.PermissionType permissionType, NativeGallery.MediaType mediaTypes)
    {
        NativeGallery.Permission permission = await NativeGallery.RequestPermissionAsync(permissionType, mediaTypes);
        Debug.Log("Permission result: " + permission);

        if (onFinish != null)
        {
            onFinish.Invoke();
        }
    }
}
