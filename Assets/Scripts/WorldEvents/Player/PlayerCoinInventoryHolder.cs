using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinInventoryHolder : MonoBehaviour
{
    public Image icon;
    public TMP_Text value;
    public string id;

    void HandleUpdateValue()
    {
        if (isActiveAndEnabled && !string.IsNullOrEmpty(id))
        {
            bool exists = ClientManager.playerData.playerBalance.Find(x => x.currencyID == id) != null;
            if (exists)
            {
                var balance = ClientManager.playerData.playerBalance.Find(x => x.currencyID == id);
                value.text = balance.currencyBalance.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            }
            else
            {
                value.text = 0.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            }

        }
    }

    private void Update()
    {
        HandleUpdateValue();
    }

}
