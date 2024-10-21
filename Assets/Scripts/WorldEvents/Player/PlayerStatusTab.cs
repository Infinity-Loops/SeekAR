using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusTab : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text balanceText;
    public TMP_Text levelText;
    public TMP_Text xpText;
    public Image avatarIcon;
    public CoinDB coins;
    public GameObject coinTemplate;
    public Transform coinContainer;

    void HandleBalance()
    {
        double totalBalance = 0;

        if (ClientManager.playerData.playerBalance != null)
        {
            if (ClientManager.playerData.playerBalance.Count > 0)
            {
                foreach (var currency in ClientManager.playerData.playerBalance)
                {
                    totalBalance += currency.currencyBalance;
                }
            }
        }

        balanceText.text = totalBalance.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
    }

    void HandleLevel()
    {
        levelText.text = ClientManager.playerData.playerLevel.ToString();
    }

    void HandleXP()
    {
        xpText.text = ClientManager.playerData.playerXP.ToString();
    }

    void HandlePlayerName()
    {
        nameText.text = ClientManager.playerData.playerName;
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
                    Texture2D cutTex = CutTextureToSquare(imageParsed, 512);
                    Texture2D circleTex = ApplyCircleMask(imageParsed);
                    DataSystem.avatar = Sprite.Create(circleTex, new Rect(Vector2.zero, new Vector2(imageParsed.width, imageParsed.height)), new Vector2(0.5f, 0.5f));
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
        croppedTexture.Apply(false,false);

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

        circularTexture.Apply(false,false);
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

    void HandleAddCoinsToInventoryContext()
    {
        var coinList = coins.coins;

        foreach (var coin in coinList)
        {
            var template = Instantiate(coinTemplate, coinContainer);
            var holder = template.GetComponent<PlayerCoinInventoryHolder>();
            holder.icon.sprite = coin.icon;
            holder.id = coin.name;
            template.gameObject.SetActive(true);
        }
    }

    void LoadProfilePhoto()
    {
        if(DataSystem.avatar != null)
        {
            avatarIcon.sprite = DataSystem.avatar;
        }
    }

    private void Awake()
    {
        HandleAddCoinsToInventoryContext();
        LoadProfilePhoto();
    }

    private void Update()
    {
        HandlePlayerName();
        HandleBalance();
        HandleLevel();
        HandleXP();
    }
}
