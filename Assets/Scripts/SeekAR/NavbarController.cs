using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavbarController : MonoBehaviour
{
    public NavigationSection currentSection;
    public Color selectionColor;
    public Color normalColor;
    public Image home;
    public Image dashboard;
    public Image wallet;
    public Image ar;
    public Image map;
    public Camera arCamera;

    public GameObject homeSection;
    public GameObject dashboardSection;
    public GameObject walletSection;
    public GameObject overlaySection;
    public void Navigate(int section)
    {
        currentSection = (NavigationSection)section;
        ApplyColors();
    }

    private void Awake()
    {
        ApplyColors();
    }

    void ApplyColors()
    {
        switch (currentSection)
        {
            case NavigationSection.Home:
                home.DOColor(selectionColor, 0.5f);
                dashboard.DOColor(normalColor, 0.5f);
                wallet.DOColor(normalColor, 0.5f);
                map.DOColor(normalColor, 0.5f);
                ar.DOColor(normalColor, 0.5f);

                homeSection.SetActive(true);
                dashboardSection.SetActive(false);
                walletSection.SetActive(false);
                MapSystem.instance.DisableMap();
                overlaySection.SetActive(true);
                arCamera.enabled = false;
                break;
            case NavigationSection.Dashboard:
                home.DOColor(normalColor, 0.5f);
                dashboard.DOColor(selectionColor, 0.5f);
                wallet.DOColor(normalColor, 0.5f);
                map.DOColor(normalColor, 0.5f);
                ar.DOColor(normalColor, 0.5f);

                homeSection.SetActive(false);
                dashboardSection.SetActive(true);
                walletSection.SetActive(false);
                MapSystem.instance.DisableMap();
                overlaySection.SetActive(true);
                arCamera.enabled = false;
                break;
            case NavigationSection.Wallet:
                home.DOColor(normalColor, 0.5f);
                dashboard.DOColor(normalColor, 0.5f);
                wallet.DOColor(selectionColor, 0.5f);
                map.DOColor(normalColor, 0.5f);
                ar.DOColor(normalColor, 0.5f);

                MapSystem.instance.DisableMap();
                homeSection.SetActive(false);
                dashboardSection.SetActive(false);
                walletSection.SetActive(true);
                overlaySection.SetActive(true);
                arCamera.enabled = false;
                break;
            case NavigationSection.Map:
                home.DOColor(normalColor, 0.5f);
                dashboard.DOColor(normalColor, 0.5f);
                wallet.DOColor(normalColor, 0.5f);
                map.DOColor(selectionColor, 0.5f);
                ar.DOColor(normalColor, 0.5f);

                MapSystem.instance.EnableMap();
                homeSection.SetActive(false);
                dashboardSection.SetActive(false);
                walletSection.SetActive(false);
                overlaySection.SetActive(false);
                arCamera.enabled = false;
                break;
            case NavigationSection.AR:
                home.DOColor(normalColor, 0.5f);
                dashboard.DOColor(normalColor, 0.5f);
                wallet.DOColor(normalColor, 0.5f);
                map.DOColor(normalColor, 0.5f);
                ar.DOColor(selectionColor, 0.5f);
                homeSection.SetActive(false);
                dashboardSection.SetActive(false);
                walletSection.SetActive(false);
                overlaySection.SetActive(false);
                MapSystem.instance.DisableMap();
                arCamera.enabled = true;
                break;
        }
    }
}

public enum NavigationSection
{
    Home,
    Dashboard,
    Wallet,
    AR,
    Map
}