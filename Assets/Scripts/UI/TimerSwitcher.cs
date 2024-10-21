
using System.Collections.Generic;
using UnityEngine;

public class TimerSwitcher : MonoBehaviour
{
    public List<TimerObject> timers = new List<TimerObject>();

    void HandleTimersState()
    {
        foreach (var timer in timers)
        {
            switch (timer.mode)
            {
                case TimerMode.PowerUp_PumpIt:
                    timer.gobject.SetActive(PowerupData.pumpItActive);
                    break;
                case TimerMode.PowerUp_ToTheMoon:
                    timer.gobject.SetActive(PowerupData.toTheMoonActive);
                    break;
                case TimerMode.PowerUp_ShowMeTheMoney:
                    timer.gobject.SetActive(PowerupData.showMeTheMoneyActive);
                    break;
                case TimerMode.PowerUp_CoinMagnet:
                    timer.gobject.SetActive(PowerupData.magnetActive);
                    break;
                case TimerMode.PowerUp_FitnessMultiplier:
                    timer.gobject.SetActive(PowerupData.fitnessMultiplierActive);
                    break;
            }
        }
    }

    void Update()
    {
        HandleTimersState();
    }
}

[System.Serializable]
public class TimerObject
{
    public TimerMode mode;
    public GameObject gobject;
}