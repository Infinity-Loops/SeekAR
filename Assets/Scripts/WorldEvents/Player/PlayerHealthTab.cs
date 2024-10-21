using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthTab : MonoBehaviour
{
    public AudioClip welcomeHealthTab;
    public TMP_Text stepText;
    public TMP_Text distanceText;
    public TMP_Text caloriesText;
    private double stepsCount;
    private float distanceCount;
    private double caloriesCount;
    private double height = 1.75;
    private double weight = 80;

    void HandleReadHealthData()
    {
        stepsCount = HealthManager.steps;
        distanceCount = HealthManager.totalDistanceKM;
    }


    public double CalculateBurnedCalories(int steps, double height, double weight, double calorieFactorPerKm)
    {
        // Calculate the average step length (in meters)
        double stepLength = height * 0.413; // For men, use 0.413, for women, you could use 0.415

        // Calculate the distance walked (in km)
        double distanceWalked = (steps * stepLength) / 1000;

        // Calculate the burned calories
        double burnedCalories = distanceWalked * weight * calorieFactorPerKm;

        return burnedCalories;
    }

    void HandleCalculateCalories()
    {
        caloriesCount = CalculateBurnedCalories(Mathf.RoundToInt((float)stepsCount), height, weight, 0.5);
    }

    void HandleDisplaySteps()
    {
        stepText.text = Mathf.Round((float)stepsCount).ToString();
    }

    void HandleDisplayDistance()
    {
        distanceText.text = $"{distanceCount.ToString("F2")} KM";
    }

    void HandleDisplayCalories()
    {
        caloriesText.text = $"{caloriesCount.ToString()} kcal";
    }

    void HandleHealthTabWelcomeAudio()
    {
        if (!DataSystem.gameData.isHealthTabInitialAudioPlayed)
        {
            AudioUtils.PlayClipAtPoint(welcomeHealthTab, transform.position);
            DataSystem.gameData.isHealthTabInitialAudioPlayed = true;
            DataSystem.SaveData();
        }
    }

    private void OnEnable()
    {
        HandleReadHealthData();
        HandleHealthTabWelcomeAudio();
    }

    private void Update()
    {
        HandleCalculateCalories();
        HandleDisplaySteps();
        HandleDisplayDistance();
        HandleDisplayCalories();
    }
}
