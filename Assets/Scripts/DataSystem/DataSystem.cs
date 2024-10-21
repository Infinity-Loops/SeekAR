using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class DataSystem
{
    private static string dataPath = $"{Application.persistentDataPath}/sav.json";
    private static string photoPath = $"{Application.persistentDataPath}/photo.dat";

    public static GameData gameData = new GameData();
    public static Sprite avatar;
    public static void SaveData()
    {
        string jsonCode = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(dataPath, jsonCode);
    }

    public static void SaveAvatarPhoto()
    {
        if (avatar != null)
        {
            var tex = avatar.texture;
            var encodedBytes = tex.EncodeToPNG();
            File.WriteAllBytes(photoPath, encodedBytes);
        }
    }
    public static void ReadData()
    {
        if (File.Exists(dataPath))
        {
            gameData = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(dataPath));
        }

        if (File.Exists(photoPath))
        {
            byte[] imageContent = File.ReadAllBytes(photoPath);
            Texture2D imageParsed = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            imageParsed.LoadImage(imageContent);
            imageParsed.Apply(false, false);
            avatar = Sprite.Create(imageParsed, new Rect(Vector2.zero, new Vector2(imageParsed.width, imageParsed.height)), new Vector2(0.5f, 0.5f));
        }
    }
}
