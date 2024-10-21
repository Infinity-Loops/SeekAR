using DG.Tweening;
using Niantic.Lightship.Maps.Core.Coordinates;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombDefuseObject : MonoBehaviour
{
    public float timeToDefuseSpeed;
    public string targetDefuseCode;
    public float targetCPUDefuseState;
    public Potentiometer firstPotentiometer;
    public Potentiometer secondPotentiometer;
    public NftDB nfts;
    public TMP_Text clockText;
    public bool bombIsRunning;
    private bool timeHasEnded;
    public Light runningLight;
    public Light cpuPowerLight;
    public Light unlockLight;
    public Light cpuDefuseStateLight;
    public Light batteryOffLight;
    public Light wireCutLight;
    public Light bombDefusedLight;
    public ParticleSystem explosionParticle;
    public GameObject defusedInterface;
    private bool gameEnded;
    private float totalSeconds;
    public BombDefuseState bombDefuseState = new BombDefuseState();
    private Nft choosenRandomReward;
    private bool isCloseEnough;
    private GameObject instantiatedMapObject;
    public Transform startMenu;
    public Transform preview;
    public Transform game;
    private bool gameStarted;
    void HandleClock()
    {
        if (!bombDefuseState.defused)
        {
            totalSeconds = Mathf.Clamp(totalSeconds - (Time.deltaTime * timeToDefuseSpeed + (bombDefuseState.timeMultiplier * 1)), 0, totalSeconds);
        }

        int hours = Mathf.FloorToInt(totalSeconds / 3600);

        int minutes = Mathf.FloorToInt((totalSeconds % 3600) / 60);

        int seconds = Mathf.FloorToInt(totalSeconds % 60);

        clockText.text = $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";

        bombIsRunning = totalSeconds > 0;

        if (!bombIsRunning && !timeHasEnded)
        {
            timeHasEnded = true;
            var explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            explosion.Play();

            Destroy(gameObject);
        }
    }

    void HandleLights()
    {
        if (bombIsRunning)
        {
            runningLight.intensity = Mathf.Lerp(runningLight.intensity, 1f, 5 * Time.deltaTime);
        }
        else
        {
            runningLight.intensity = Mathf.Lerp(runningLight.intensity, 0f, 5 * Time.deltaTime);
        }

        if (bombDefuseState.cpuPower)
        {
            cpuPowerLight.intensity = Mathf.Lerp(cpuPowerLight.intensity, 1f, 5 * Time.deltaTime);
        }
        else
        {
            cpuPowerLight.intensity = Mathf.Lerp(cpuPowerLight.intensity, 0f, 5 * Time.deltaTime);
        }

        if (bombDefuseState.codeMatches)
        {
            unlockLight.intensity = Mathf.Lerp(unlockLight.intensity, 1f, 5 * Time.deltaTime);
        }
        else
        {
            unlockLight.intensity = Mathf.Lerp(unlockLight.intensity, 0f, 5 * Time.deltaTime);
        }

        if (bombDefuseState.cpuDefuseStateMatches)
        {
            cpuDefuseStateLight.intensity = Mathf.Lerp(cpuDefuseStateLight.intensity, 1f, 5 * Time.deltaTime);
        }
        else
        {
            cpuDefuseStateLight.intensity = Mathf.Lerp(cpuDefuseStateLight.intensity, 0f, 5 * Time.deltaTime);
        }

        if (bombDefuseState.batteryOff)
        {
            batteryOffLight.intensity = Mathf.Lerp(batteryOffLight.intensity, 1f, 5 * Time.deltaTime);
        }
        else
        {
            batteryOffLight.intensity = Mathf.Lerp(batteryOffLight.intensity, 0f, 5 * Time.deltaTime);
        }

        if (bombDefuseState.wireCut)
        {
            wireCutLight.intensity = Mathf.Lerp(wireCutLight.intensity, 1f, 5 * Time.deltaTime);
        }
        else
        {
            wireCutLight.intensity = Mathf.Lerp(wireCutLight.intensity, 0f, 5 * Time.deltaTime);
        }


        if (bombDefuseState.defused)
        {
            bombDefusedLight.intensity = Mathf.Lerp(bombDefusedLight.intensity, 1f, 5 * Time.deltaTime);
        }
        else
        {
            bombDefusedLight.intensity = Mathf.Lerp(bombDefusedLight.intensity, 0f, 5 * Time.deltaTime);
        }
    }

    void HandleDefuseState()
    {
        bombDefuseState.cpuDefuseState = firstPotentiometer.value;
        bombDefuseState.timeMultiplier = secondPotentiometer.value;
        bombDefuseState.codeMatches = bombDefuseState.currentCode == targetDefuseCode;
        bombDefuseState.cpuDefuseStateMatches = Mathf.Round(bombDefuseState.cpuDefuseState) == Mathf.Round(targetCPUDefuseState);
        bombDefuseState.defused = bombDefuseState.cpuPower && bombDefuseState.codeMatches && bombDefuseState.cpuDefuseStateMatches && bombDefuseState.batteryOff && bombDefuseState.wireCut;

        if (bombDefuseState.defused && !gameEnded)
        {
            gameEnded = true;
            defusedInterface.SetActive(true);
            defusedInterface.transform.localScale = Vector3.zero;
            defusedInterface.transform.DOScale(Vector3.one * 0.003f, 0.25f);
            choosenRandomReward = nfts.NftList[Random.Range(0, nfts.NftList.Count)];
            var defusedIntComponent = defusedInterface.GetComponent<DefusedInterface>();
            defusedIntComponent.choosenReward = choosenRandomReward;
            defusedIntComponent.icon.sprite = choosenRandomReward.icon;
        }
    }

    void HandleRandomTotalTime()
    {
        totalSeconds = Random.Range(240, 360);
    }

    public void ToggleCPUPower(bool state)
    {
        bombDefuseState.cpuPower = !state;
    }

    public void WriteToCode(string character)
    {
        if (bombDefuseState.currentCode.Length > targetDefuseCode.Length)
        {
            bombDefuseState.currentCode = "";
        }

        bombDefuseState.currentCode += character;
    }

    public void CutYellowWire()
    {
        bombDefuseState.wireCut = true;
    }

    public void RemoveBattery()
    {
        bombDefuseState.batteryOff = true;
    }

    void HandleRotation()
    {
        float distance = Vector3.Distance(ARCamera.arCamera.transform.position, transform.position);
        if (distance <= 3)
        {
            if (!isCloseEnough)
            {
                isCloseEnough = true;
                transform.DORotate(new Vector3(0, ARCamera.arCamera.transform.eulerAngles.y, 0), 0.25f);
            }
        }
        else
        {
            isCloseEnough = false;
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        startMenu.DOScale(0, 0.25f).OnComplete(() =>
        {
            startMenu.gameObject.SetActive(false);
        });
        preview.gameObject.SetActive(false);
        game.gameObject.SetActive(true);
    }

    private void Awake()
    {
        HandleRandomTotalTime();
    }

    private void Update()
    {

        HandleRotation();

        if (!gameStarted)
        {
            return;
        }

        HandleClock();
        HandleLights();
        HandleDefuseState();
    }

    private void OnDestroy()
    {
        if (instantiatedMapObject != null)
        {
            Destroy(instantiatedMapObject);
        }
    }
}

[Serializable]
public class BombDefuseState
{
    public bool cpuPower;
    public float cpuDefuseState;
    public bool cpuDefuseStateMatches;
    public float timeMultiplier;
    public string currentCode;
    public bool codeMatches;
    public bool wireCut;
    public bool batteryOff;
    public bool defused;
}