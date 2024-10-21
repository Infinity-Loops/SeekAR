using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeShareNamespace;
using ReadyPlayerMe.WebView;
public class MainHotbarHandler : MonoBehaviour
{
    public GameObject homeHUD;
    public GameObject gameInterface;
    public GameObject screenshotLogo;
    public GameObject menu;
    public WebViewPanel readyPlayerMeWebviewPanel;
    private GameObject spawnedHomeHUD;

    public void ShowMenu()
    {
        if (menu.activeInHierarchy)
        {
            menu.transform.localScale = Vector3.one;
            menu.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
            {
                menu.gameObject.SetActive(false);
            });
            return;
        }
        else
        {
            menu.gameObject.SetActive(true);
            menu.transform.localScale = Vector3.zero;
            menu.transform.DOScale(Vector3.one, 0.25f);
        }
    }

    public void ShowAvatarEditor()
    {
        ShowMenu();
        readyPlayerMeWebviewPanel.LoadWebView(DataSystem.gameData.playerData.readyPlayerMeAuthToken);
    }

    public void ShowHomeHUD()
    {
#if GEOQUEST
        if (spawnedHomeHUD == null)
        {
            Vector3 location = ARCamera.arCamera.transform.position + ARCamera.arCamera.transform.forward * 2f;
            Quaternion rotation = Quaternion.Euler(0, ARCamera.arCamera.transform.eulerAngles.y, 0);
            spawnedHomeHUD = Instantiate(homeHUD, location, rotation);
            spawnedHomeHUD.transform.localScale = Vector3.zero;
            spawnedHomeHUD.transform.DOScale(Vector3.one * 0.003f, 0.25f);
        }
        else
        {
            spawnedHomeHUD.GetComponent<PlayerGameMenu>().Close();
        }
#endif
    }

    public void TakePhoto()
    {
        StartCoroutine(HandleTakePhoto());
    }

    IEnumerator HandleTakePhoto()
    {
        gameInterface.SetActive(false);
        screenshotLogo.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        var shot = ScreenCapture.CaptureScreenshotAsTexture();
        var share = new NativeShare();
        share.AddFile(shot);
        share.Share();
        yield return new WaitForSeconds(0.25f);
        gameInterface.SetActive(true);
        screenshotLogo.SetActive(false);
    }
}
