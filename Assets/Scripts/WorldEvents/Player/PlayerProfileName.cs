using TMPro;
using UnityEngine;

public class PlayerProfileName : MonoBehaviour
{
    public TMP_InputField input;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(DataSystem.gameData.playerData.playerName))
        {
            input.text = DataSystem.gameData.playerData.playerName;
        }
    }

    public void EndEdit()
    {
        DataSystem.gameData.playerData.playerName = input.text;
        DataSystem.SaveData();
    }
}
