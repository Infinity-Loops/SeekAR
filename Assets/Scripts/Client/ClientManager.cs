using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngineInternal;

public class ClientManager : MonoBehaviour
{
    public static ClientManager instance;
    public static DateTime startTime;
    public static PlayerData playerData = new PlayerData();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        GameObject clientManager = new GameObject("ClientManager", typeof(ClientManager));
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        HandleSetupInitialServices();
        HandleInitialSettings();
        HandleSetupOptimizationSettings();
    }

    void HandleSetupInitialServices()
    {
        GeolocationInput.Init();
        DataSystem.ReadData();
        playerData = DataSystem.gameData.playerData;

        if(playerData.playerBalance == null)
        {
            playerData.playerBalance = new List<CurrencyData>();
            DataSystem.SaveData();
        }
    }

    void HandleSetupOptimizationSettings()
    {
        Application.targetFrameRate = 30;
        Physics.autoSyncTransforms = false;
    }

    void HandleInitialSettings()
    {
        startTime = DateTime.Now;
        EnhancedTouchSupport.Enable();
    }
}
