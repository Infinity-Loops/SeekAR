using TMPro;
using UnityEngine;

public class VisualTimer : MonoBehaviour
{
    public TMP_Text text;
    public TimerMode mode;
    private float remainingTime;

    void HandleTime()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void HandleMode()
    {
        switch (mode)
        {
            case TimerMode.PowerUp_PumpIt:
                remainingTime = PowerupManager.instance.pumpItTimer;
                break;
            case TimerMode.PowerUp_ToTheMoon:
                remainingTime = PowerupManager.instance.toTheMoonTimer;
                break;
            case TimerMode.PowerUp_ShowMeTheMoney:
                remainingTime = PowerupManager.instance.showMeTheMoneyTimer;
                break;
            case TimerMode.PowerUp_CoinMagnet:
                remainingTime = PowerupManager.instance.magnetTimer;
                break;
            case TimerMode.PowerUp_FitnessMultiplier:
                remainingTime = PowerupManager.instance.fitnessMultiplierTimer;
                break;
        }
    }

    void Update()
    {
        HandleMode();
        HandleTime();
    }
}

public enum TimerMode
{
    PowerUp_PumpIt,
    PowerUp_ToTheMoon,
    PowerUp_ShowMeTheMoney,
    PowerUp_CoinMagnet,
    PowerUp_FitnessMultiplier
}