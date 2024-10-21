using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;

    internal float pumpItTimer = 300;
    internal float magnetTimer = 120;
    internal float toTheMoonTimer = 300;
    internal float showMeTheMoneyTimer = 600;
    internal float fitnessMultiplierTimer = 900;

    public GameObject radius;
    public GameObject magnetRadius;
    public GameObject extendedRadius;
    public AudioClip cantSelectAudio;

    private void Awake()
    {
        instance = this;
    }

    public static void PlayCantSelect()
    {
        AudioUtils.PlayClipAtPoint(instance.cantSelectAudio, Vector3.zero, 1f, 0f, 1f);
    }

    private void Update()
    {
        HandlePumpIt();
        HandleHODL();
        HandleToTheMoon();
        HandleShowMeTheMoney();
        HandleMagnet();
        HandleFitnessMultiplier();
    }

    private void HandleFitnessMultiplier()
    {
        if (PowerupData.fitnessMultiplierActive)
        {
            fitnessMultiplierTimer -= Time.deltaTime;
            if (fitnessMultiplierTimer <= 0)
            {
                PowerupData.fitnessMultiplierActive = false;
                RestoreFitnessMultiplier();
            }
        }
    }

    private void HandleMagnet()
    {
        if (PowerupData.magnetActive)
        {
            magnetTimer -= Time.deltaTime;
            if (magnetTimer <= 0)
            {
                PowerupData.magnetActive = false;
                RestoreMagnet();
            }
        }

        magnetRadius.SetActive(PowerupData.magnetActive);
    }

    private void HandleShowMeTheMoney()
    {
        if (PowerupData.showMeTheMoneyActive)
        {
            showMeTheMoneyTimer -= Time.deltaTime;

            if (showMeTheMoneyTimer <= 0)
            {
                PowerupData.showMeTheMoneyActive = false;
                RestoreShowMeTheMoney();
            }

            radius.transform.localScale = new Vector3(140, 140, 140);
        }
        else
        {
            radius.transform.localScale = new Vector3(70, 70, 70);
        }

        extendedRadius.SetActive(PowerupData.showMeTheMoneyActive);
    }

    private void HandleToTheMoon()
    {
        if (PowerupData.toTheMoonActive)
        {
            toTheMoonTimer -= Time.deltaTime;

            if (toTheMoonTimer <= 0)
            {
                PowerupData.toTheMoonActive = false;
                RestoreToTheMoon();
            }
        }
    }

    private void HandleHODL()
    {
        if (PowerupData.hodlActive)
        {
            PowerupData.hodlActive = false;

            if (DataSystem.gameData.playerData.playerBalance.Count > 0)
            {
                foreach (var balance in DataSystem.gameData.playerData.playerBalance)
                {
                    balance.currencyBalance *= 1.25f;
                }
            }
        }
    }

    private void HandlePumpIt()
    {
        if (PowerupData.pumpItActive)
        {
            pumpItTimer -= Time.deltaTime;

            if (pumpItTimer <= 0)
            {
                PowerupData.pumpItActive = false;
                RestorePumpIt();
            }
        }
    }

    private void RestoreFitnessMultiplier()
    {
        fitnessMultiplierTimer = 900;
    }

    private void RestoreMagnet()
    {
        magnetTimer = 120;
    }

    private void RestoreShowMeTheMoney()
    {
        showMeTheMoneyTimer = 600;
    }

    private void RestoreToTheMoon()
    {
        toTheMoonTimer = 300;
    }

    private void RestorePumpIt()
    {
        pumpItTimer = 300;
    }
}
