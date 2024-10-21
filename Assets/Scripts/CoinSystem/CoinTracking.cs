using UnityEngine;

public static class CoinTracking
{
    public static void AddCoinValue(string coin, float value)
    {
        Debug.Log($"Adding {value} to ${coin}");

        bool exists = ClientManager.playerData.playerBalance.Find(x => x.currencyID == coin) != null;

        if (!exists)
        {
            CurrencyData data = new CurrencyData();
            data.currencyBalance = value;
            data.currencyID = coin;
            ClientManager.playerData.playerBalance.Add(data);
        }
        else
        {
            var data = ClientManager.playerData.playerBalance.Find(x => x.currencyID == coin);
            data.currencyBalance += value;
        }

        DataSystem.SaveData();
    }
}
